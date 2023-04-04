using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using System.Reflection;
using Serilog.Events;

namespace Development.Suite.Logging;

public class SeriLogConfiguration
{
    public static void ConfigureSeriLog(LoggerConfiguration logger, LogEventLevel logEventLevel = LogEventLevel.Debug)
    {
        var assembly = Assembly.GetCallingAssembly();
        var jsonFormatter = new JsonFormatter();
        var logPath = Path.GetDirectoryName(assembly.Location) ?? ".";
        var logName = assembly.GetName()?.Name?.ToLower() ?? "log";

        logger
            .MinimumLevel.Is(logEventLevel)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .WriteTo.File(jsonFormatter, $"{logPath}/logs/{logName}.txt", rollingInterval: RollingInterval.Day);
    }
}