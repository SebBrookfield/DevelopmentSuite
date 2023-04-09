using System.Text.Json;
using Autofac;
using Development.Suite.Ipc.Common;
using Development.Suite.Logging;

namespace Development.Suite.Ipc.MessageHandling;

public interface IIpcMessageHandler<THandlerType> where THandlerType : IMessageHandler
{
    Task<IpcModel?> HandleMessage(IpcMessage message);
}

public class IpcMessageHandler<THandlerType> : IIpcMessageHandler<THandlerType> where THandlerType : IMessageHandler
{
    private readonly IComponentContext _componentContext;
    private readonly IDevelopmentSuiteLogger<IpcMessageHandler<THandlerType>> _logger;
    private readonly Type _enumerableType;
    private readonly Type _genericHandlerType;
    private readonly Dictionary<Type, List<ReflectedHandler>> _handlersByType;
    private readonly ILookup<string, Type> _typesByName;

    public IpcMessageHandler(IComponentContext componentContext, IDevelopmentSuiteLogger<IpcMessageHandler<THandlerType>> logger)
    {
        var ipcModelType = typeof(IpcModel);

        _componentContext = componentContext;
        _logger = logger;
        _enumerableType = typeof(IEnumerable<>);
        _genericHandlerType = typeof(THandlerType) == typeof(IServiceMessageHandler)
            ? typeof(IServiceMessageHandler<>)
            : typeof(IClientMessageHandler<>);
        _handlersByType = new Dictionary<Type, List<ReflectedHandler>>();
        _typesByName = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsAssignableTo(ipcModelType) && t != ipcModelType)
            .ToLookup(t => t.FullName ?? t.Name);
    }

    public async Task<IpcModel?> HandleMessage(IpcMessage message)
    {
        var type = _typesByName[message.FullName].FirstOrDefault();

        if (type == null)
        {
            _logger.LogWarning("Skipping message as type cannot be found.", message);
            return null;
        }

        if (JsonSerializer.Deserialize(message.Message, type) is not IpcModel deserializedMessage)
        {
            _logger.LogWarning("Deserialized message was not an IpcModel.");
            return null;
        }

        var handleTasks = ResolveHandlers(type).Select(handler => Handle(handler, deserializedMessage));

        await Task.WhenAll(handleTasks);

        return deserializedMessage;
    }

    private async Task Handle(ReflectedHandler handler, IpcModel message)
    {
        _logger.LogDebug($"Calling {handler.Name}.{nameof(ReflectedHandler.HandleMessage)}");
        try
        {
            await handler.HandleMessage(message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to handle message.");
        }
    }

    private IEnumerable<ReflectedHandler> ResolveHandlers(Type type)
    {
        if (_handlersByType.ContainsKey(type))
            return _handlersByType[type];

        try
        {
            var handlerType = _genericHandlerType.MakeGenericType(type);
            var enumerableHandlerType = _enumerableType.MakeGenericType(handlerType);

            if (_componentContext.Resolve(enumerableHandlerType) is not IEnumerable<object> handlers)
                return _handlersByType[type] = new List<ReflectedHandler>();

            return _handlersByType[type] = handlers
                .Select(h => new ReflectedHandler(h))
                .ToList();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Failed to resolve handlers for type {type.FullName ?? type.Name}.");
            return Enumerable.Empty<ReflectedHandler>();
        }
    }
}