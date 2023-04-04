using Development.Suite.Ipc;
using Development.Suite.Logging;

namespace Development.Suite.Service;

public class IpcWorker : BackgroundService
{
    private readonly IDevelopmentSuiteLogger<IpcWorker> _logger;
    private readonly IIpcServer _ipcServer;
    private readonly IIpcMessageHandler _ipcMessageHandler;

    public IpcWorker(IDevelopmentSuiteLogger<IpcWorker> logger, IIpcServer ipcServer, IIpcMessageHandler ipcMessageHandler)
    {
        _logger = logger;
        _ipcServer = ipcServer;
        _ipcMessageHandler = ipcMessageHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Starting");
        await _ipcServer.Start(stoppingToken);

        foreach (var message in _ipcServer.Messages)
        {
            _logger.LogDebug("Received {@message}", message);
            _ipcMessageHandler.HandleMessage(message);
            _logger.LogDebug("Waiting for next message");
        }

        _logger.LogDebug("Stopped.");
    }
}