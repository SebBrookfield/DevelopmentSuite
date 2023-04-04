using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Development.Suite.Logging
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddLogging(this IHostBuilder hostBuilder, LogEventLevel logEventLevel = LogEventLevel.Debug)
        {
            return hostBuilder.UseSerilog((_, logger) => SeriLogConfiguration.ConfigureSeriLog(logger, logEventLevel))
                .ConfigureServices((_, collection) => collection.AddSingleton(typeof(IDevelopmentSuiteLogger<>), typeof(DevelopmentSuiteLogger<>)));
        }
    }
}
