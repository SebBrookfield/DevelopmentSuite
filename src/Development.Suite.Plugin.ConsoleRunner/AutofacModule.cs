using Autofac;

namespace Development.Suite.Plugin.ConsoleRunner;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ConsoleMessageHandler>().As<IMessageHandler<ConsoleMessage>>();
    }
}