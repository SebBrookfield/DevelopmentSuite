using System.Net;
using System.Net.Sockets;
using Development.Suite.Logging;
using Microsoft.Extensions.Options;

namespace Development.Suite.Ipc.Tcp;

public class TcpIpcServer : TcpIpcBase, IIpcServer
{
    private readonly TcpListener _listener;

    public TcpIpcServer(IOptions<TcpIpcConfig> config, IDevelopmentSuiteLogger<TcpIpcServer> logger) : base(config.Value, logger)
    {
        _listener = new TcpListener(IPAddress.Loopback, TcpIpcConfig.Port);
    }

    public override async Task Start(CancellationToken cancellationToken)
    {
        _listener.Start();
        await base.Start(cancellationToken);
    }

    protected override async Task<TcpClient> GetTcpClient()
    {
        Logger.LogDebug("Waiting for client");
        var client = await _listener.AcceptTcpClientAsync(CancellationToken);
        Logger.LogDebug("Client connected");
        return client;
    }
}