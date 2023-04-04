using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Development.Suite.Ipc.Tcp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTcpIpcServer(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTcpIpcConfig(configuration)
            .AddSingleton<IIpcServer, TcpIpcServer>();
    }

    public static IServiceCollection AddTcpIpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTcpIpcConfig(configuration)
            .AddSingleton<IIpcClient, TcpIpcClient>();
    }

    private static IServiceCollection AddTcpIpcConfig(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<TcpIpcConfig>(configuration.GetSection("IpcConfig:Tcp"));
    }
}