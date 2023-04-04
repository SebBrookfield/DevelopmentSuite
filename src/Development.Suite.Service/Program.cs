using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Development.Suite.Ipc.Tcp;
using Development.Suite.Logging;

namespace Development.Suite.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
                    foreach (var assemblyPath in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
                    {
                        var assemblyBytes = File.ReadAllBytes(assemblyPath);
                        var assembly = Assembly.Load(assemblyBytes);
                        builder.RegisterAssemblyModules(assembly);
                    }

                    builder.RegisterType<HandlerResolver>();

                })
                .UseWindowsService(config =>
                {
                    config.ServiceName = "AIQ Development Suite Service";
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<IpcWorker>();
                    services.AddTcpIpcServer(context.Configuration);
                })
                .AddLogging()
                .Build()
                .Run();
        }
    }
}