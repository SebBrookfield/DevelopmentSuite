using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Development.Suite.Logging
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((_, logger) => SeriLogConfiguration.ConfigureSeriLog(logger))
                .ConfigureServices((_, collection) => collection.AddSingleton(typeof(IDevelopmentSuiteLogger<>), typeof(DevelopmentSuiteLogger<>)));
        }
    }
}
