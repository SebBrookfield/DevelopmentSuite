using Autofac;

namespace Development.Suite.App.Plugin.ExampleCommand;

// ReSharper disable once UnusedType.Global
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ExampleCommand>().As<IPluginCommand>();
    }
}