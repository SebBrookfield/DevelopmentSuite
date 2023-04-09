using Autofac;
using Autofac.Extensions.DependencyInjection;
using Development.Suite.Common.ExtensionMethods;
using Development.Suite.Ipc.Common;
using Development.Suite.Ipc.MessageHandling;
using Development.Suite.Ipc.Tcp;
using Development.Suite.Logging;

namespace Development.Suite.Service
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.LoadPlugins();
                })
                .UseWindowsService(config =>
                {
                    config.ServiceName = "AIQ Development Suite Service";
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<IpcWorker>();
                    services.AddTcpIpcServer(context.Configuration);
                    services.AddSingleton<IIpcMessageHandler, IpcMessageHandler>();
                    services.AddSingleton<IIpcMessageSender, IpcMessageSender>();
                })
                .AddLogging()
                .Build()
                .Run();
        }
    }
}