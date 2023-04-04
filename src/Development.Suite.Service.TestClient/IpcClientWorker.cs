using Development.Suite.Ipc;
using Development.Suite.Ipc.MessageHandling;
using Development.Suite.Logging;
using Microsoft.Extensions.Hosting;

namespace Development.Suite.Service.TestClient;

public class IpcClientWorker : BackgroundService
{
    private readonly IDevelopmentSuiteLogger<IpcClientWorker> _logger;
    private readonly IIpcClient _ipcClient;
    private readonly IIpcMessageHandler _ipcMessageHandler;

    public IpcClientWorker(IDevelopmentSuiteLogger<IpcClientWorker> logger, IIpcClient ipcClient, IIpcMessageHandler ipcMessageHandler)
    {
        _logger = logger;
        _ipcClient = ipcClient;
        _ipcMessageHandler = ipcMessageHandler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => Execute(stoppingToken), stoppingToken);
    }

    private async Task Execute(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Starting");
        await _ipcClient.Start(stoppingToken);

        foreach (var message in _ipcClient.Messages)
        {
            _logger.LogDebug("Received {@message}", message);
            _ipcMessageHandler.HandleMessage(message);
            _logger.LogDebug("Waiting for next message");
        }

        _logger.LogDebug("Stopped.");
    }
}