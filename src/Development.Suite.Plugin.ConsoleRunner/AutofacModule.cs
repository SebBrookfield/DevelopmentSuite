using Autofac;

namespace Development.Suite.Plugin.ConsoleRunner;

// ReSharper disable once UnusedType.Global
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ConsoleMessageHandler>().As<IMessageHandler<ConsoleMessage>>();
    }
}