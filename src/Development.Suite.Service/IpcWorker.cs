using System.Text.Json;
using Development.Suite.Ipc;
using Development.Suite.Logging;

namespace Development.Suite.Service;

public class IpcWorker : BackgroundService
{
    private readonly IDevelopmentSuiteLogger<IpcWorker> _logger;
    private readonly IIpcServer _ipcServer;
    private readonly HandlerResolver _handlerResolver;
    private readonly ILookup<string, Type> _typesByName;

    public IpcWorker(IDevelopmentSuiteLogger<IpcWorker> logger, IIpcServer ipcServer, HandlerResolver handlerResolver)
    {
        _logger = logger;
        _ipcServer = ipcServer;
        _handlerResolver = handlerResolver;
        _typesByName = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .ToLookup(t => t.FullName ?? t.Name);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Starting");
        await _ipcServer.Start(stoppingToken);

        foreach (var message in _ipcServer.Messages)
        {
            _logger.LogDebug("Received {@message}", message);

            var type = _typesByName[message.FullName]?.FirstOrDefault();

            if (type == null)
            {
                _logger.LogWarning("Skipping message as type cannot be found.", message);
                continue;
            }

            var deserializedMessage = JsonSerializer.Deserialize(message.Message, type);

            if (deserializedMessage == null)
            {
                _logger.LogWarning("Deserialized message was null.");
                continue;
            }

            var handlers = _handlerResolver.ResolveHandlers(type);

            foreach (var handler in handlers)
            {
                _logger.LogDebug($"Calling {handler.Name}.{nameof(ReflectedHandler.HandleMessage)}");
                try
                {
                    handler.HandleMessage(deserializedMessage);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Failed to handle message.");
                }
            }

            _logger.LogDebug("Waiting for next message");
        }

        _logger.LogDebug("Stopped.");
    }
}