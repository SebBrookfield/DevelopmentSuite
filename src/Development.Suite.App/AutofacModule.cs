using Autofac;
using Development.Suite.App.ViewModels;
using Development.Suite.Logging;
using Serilog.Events;

namespace Development.Suite.App;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterLogging(LogEventLevel.Debug);
        builder.RegisterType<MainViewModel>();
    }
}