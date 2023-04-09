using Autofac;
using Development.Suite.App.Plugin;
using Development.Suite.App.Utilities;
using Development.Suite.App.ViewModels;
using Development.Suite.Common.ExtensionMethods;
using Development.Suite.Ipc;
using Development.Suite.Ipc.Tcp;
using Development.Suite.Ipc.MessageHandling;
using Development.Suite.Logging;
using Microsoft.Extensions.Options;
using Serilog.Events;
using Module = Autofac.Module;
using Development.Suite.Ipc.Common;

namespace Development.Suite.App;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.LoadPlugins();

        builder.Register(c => Options.Create(new TcpIpcConfig() { Port = 5388 }));
        builder.RegisterType<TcpIpcClient>().SingleInstance();
        builder.Register(c => c.Resolve<TcpIpcClient>()).As<IIpcClient>();
        builder.Register(c => c.Resolve<TcpIpcClient>()).As<IIpcSender>();
        builder.RegisterType<IpcMessageSender>().As<IIpcMessageSender>().SingleInstance();
        builder.RegisterType<IpcMessageHandler>().As<IIpcMessageHandler>().SingleInstance();
        builder.RegisterType<Messenger>().As<IMessenger>().SingleInstance();
        builder.RegisterType<IpcClient>().SingleInstance();

        builder.RegisterLogging(LogEventLevel.Debug);
        builder.RegisterType<MainViewModel>();
    }
}