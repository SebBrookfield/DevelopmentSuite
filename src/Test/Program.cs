using Development.Suite.Ipc;
using Development.Suite.Ipc.Tcp;
using Development.Suite.Logging;
using Development.Suite.Plugin.ConsoleRunner;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;

namespace Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serilogLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var tcpLogger = new SerilogLoggerFactory(serilogLogger).CreateLogger<TcpIpcClient>();
            var devLogger = new DevelopmentSuiteLogger<TcpIpcClient>(tcpLogger);
            var programLogger = new SerilogLoggerFactory(serilogLogger).CreateLogger<Program>();

            var cancellationTokenSource = new CancellationTokenSource();
            var ipcConfig = new TcpIpcConfig { Port = 5389 };
            var client = new TcpIpcClient(Options.Create(ipcConfig), devLogger);
            await client.Start(cancellationTokenSource.Token);

            Task.Factory.StartNew(() =>
            {
                foreach (var message in client.Messages)
                    programLogger.LogDebug("client rx: {message}", message);
            });

            while (true)
            {
                var line = Console.ReadLine();
                var a = new ConsoleMessage
                {
                    Command = line
                };
                
                programLogger.LogDebug("client tx: {line}", line);
                client.Send(IpcMessage.ToIpcMessage(a));
            }
        }
    }
}