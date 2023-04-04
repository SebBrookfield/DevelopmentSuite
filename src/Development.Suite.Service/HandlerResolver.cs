using Autofac;
using Development.Suite.Logging;
using Development.Suite.Plugin;

namespace Development.Suite.Service;

public class HandlerResolver
{
    private readonly IComponentContext _componentContext;
    private readonly IDevelopmentSuiteLogger<HandlerResolver> _logger;
    private readonly Type _enumerableType;
    private readonly Type _genericHandlerType;
    private readonly Dictionary<Type, List<ReflectedHandler>> _handlersByType;

    public HandlerResolver(IComponentContext componentContext, IDevelopmentSuiteLogger<HandlerResolver> logger)
    {
        _componentContext = componentContext;
        _logger = logger;
        _enumerableType = typeof(IEnumerable<>);
        _genericHandlerType = typeof(IMessageHandler<>);
        _handlersByType = new Dictionary<Type, List<ReflectedHandler>>();
    }

    public IEnumerable<ReflectedHandler> ResolveHandlers(Type type)
    {
        if (_handlersByType.ContainsKey(type))
            return _handlersByType[type];

        try
        {
            var handlerType = _genericHandlerType.MakeGenericType(type);
            var enumerableHandlerType = _enumerableType.MakeGenericType(handlerType);
            var handlers = _componentContext.Resolve(enumerableHandlerType) as IEnumerable<object>;

            if (handlers == null)
            {
                return _handlersByType[type] = new List<ReflectedHandler>();
            }

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