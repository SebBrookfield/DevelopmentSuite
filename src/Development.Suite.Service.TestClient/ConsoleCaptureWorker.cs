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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => Execute(stoppingToken), stoppingToken);
    }

    private Task Execute(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var line = Console.ReadLine();
            var consoleMessage = new ConsoleMessage
            {
                Command = line
            };

            _logger.LogDebug("client tx: {@consoleMessage}", consoleMessage);
            _ipcMessageSender.SendMessage(consoleMessage);
        }

        return Task.CompletedTask;
    }
}