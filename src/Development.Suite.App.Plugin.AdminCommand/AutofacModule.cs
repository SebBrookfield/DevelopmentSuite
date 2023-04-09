using Autofac;

namespace Development.Suite.App.Plugin.AdminCommand;

// ReSharper disable once UnusedType.Global
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AdminCommand>().As<IPluginCommand>();
    }
}