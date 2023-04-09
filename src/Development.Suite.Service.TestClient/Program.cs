using Autofac.Extensions.DependencyInjection;
using Development.Suite.Ipc.MessageHandling;
using Development.Suite.Ipc.Tcp;
using Development.Suite.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Autofac;
using Development.Suite.Ipc.Common;
using Development.Suite.Plugin.ConsoleRunner;
using Serilog.Events;

namespace Development.Suite.Service.TestClient
{
    internal abstract class Program
    {
        private static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<ConsoleReplyHandler>().As<IMessageHandler<ConsoleMessage>>();
                })
                .ConfigureServices((context, services) =>
                {
                    // Ensure all assemblies are loaded.
                    foreach (var assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
                        Assembly.Load(assembly.FullName);

                    services.AddHostedService<ConsoleCaptureWorker>();
                    services.AddHostedService<IpcClientWorker>();
                    services.AddTcpIpcClient(context.Configuration);
                    services.AddSingleton<IIpcMessageHandler, IpcMessageHandler>();
                    services.AddSingleton<IIpcMessageSender, IpcMessageSender>();
                })
                .AddLogging(LogEventLevel.Information)
                .Build()
                .Run();
        }
    }
}