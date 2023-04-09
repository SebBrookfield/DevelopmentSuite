using System.Threading;
using System.Threading.Tasks;
using Development.Suite.App.Plugin;
using Development.Suite.Ipc;
using Development.Suite.Ipc.MessageHandling;
using Development.Suite.Logging;

namespace Development.Suite.App.Utilities;

public class IpcClient
{
    private readonly IDevelopmentSuiteLogger<IpcClient> _logger;
    private readonly IIpcClient _ipcClient;
    private readonly IIpcMessageHandler _ipcMessageHandler;
    private readonly IMessenger _messenger;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public IpcClient(IDevelopmentSuiteLogger<IpcClient> logger, IIpcClient ipcClient, IIpcMessageHandler ipcMessageHandler, IMessenger messenger)
    {
        _logger = logger;
        _ipcClient = ipcClient;
        _ipcMessageHandler = ipcMessageHandler;
        _messenger = messenger;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async void Start()
    {
        await Start(_cancellationTokenSource.Token);
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    private async Task Start(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Starting");
        await _ipcClient.Start(stoppingToken);

        await foreach (var message in _ipcClient.Messages.WithCancellation(stoppingToken))
        {
            _logger.LogDebug("Received {@message}", message);
            var processedMessage = await _ipcMessageHandler.HandleMessage(message);
            _messenger.ReceiveMessage(processedMessage);
            _logger.LogDebug("Waiting for next message");
        }

        _logger.LogDebug("Stopped.");
    }
}