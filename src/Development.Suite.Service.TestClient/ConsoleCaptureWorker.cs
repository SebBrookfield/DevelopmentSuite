using Development.Suite.Ipc.Common;
using Development.Suite.Logging;
using Development.Suite.Plugin;
using Development.Suite.Plugin.ConsoleRunner;
using Microsoft.Extensions.Hosting;

namespace Development.Suite.Service.TestClient;

public class ConsoleCaptureWorker : BackgroundService
{
    private readonly IDevelopmentSuiteLogger<ConsoleCaptureWorker> _logger;
    private readonly IIpcMessageSender _ipcMessageSender;

    public ConsoleCaptureWorker(IDevelopmentSuiteLogger<ConsoleCaptureWorker> logger, IIpcMessageSender ipcMessageSender)
    {
        _logger = logger;
        _ipcMessageSender = ipcMessageSender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var line = await Task.Run(Console.ReadLine, stoppingToken);

            if (string.IsNullOrEmpty(line))
                continue;

            var consoleMessage = new ConsoleMessage { Command = line };
            _logger.LogDebug("Sending {@consoleMessage}", consoleMessage);
            await _ipcMessageSender.SendMessage(consoleMessage);
        }
    }
}