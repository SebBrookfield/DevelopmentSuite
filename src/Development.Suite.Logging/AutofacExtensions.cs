using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Development.Suite.Logging;

public static class AutofacExtensions
{
    public static void RegisterLogging(this ContainerBuilder containerBuilder, LogEventLevel logEventLevel)
    {
        var serviceCollection = new ServiceCollection();
        var loggerConfiguration = new LoggerConfiguration();
        SeriLogConfiguration.ConfigureSeriLog(loggerConfiguration);
        serviceCollection.AddLogging(logBuilder => logBuilder.AddSerilog(loggerConfiguration.CreateLogger()));
        serviceCollection.AddSingleton(typeof(IDevelopmentSuiteLogger<>), typeof(DevelopmentSuiteLogger<>));
        containerBuilder.Populate(serviceCollection);
    }
}