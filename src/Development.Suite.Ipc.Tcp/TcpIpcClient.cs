using System.Net;
using System.Net.Sockets;
using Development.Suite.Logging;
using Microsoft.Extensions.Options;

namespace Development.Suite.Ipc.Tcp;

public class TcpIpcClient : TcpIpcBase, IIpcClient
{
    public TcpIpcClient(IOptions<TcpIpcConfig> config, IDevelopmentSuiteLogger<TcpIpcClient> logger) : base(config.Value, logger)
    {
    }

    protected override async Task<TcpClient> GetTcpClient()
    {
        var client = new TcpClient();
        var tries = 1;

        while (!client.Connected)
        {
            try
            {
                Logger.LogDebug("Connecting");
                await client.ConnectAsync(IPAddress.Loopback, TcpIpcConfig.Port, CancellationToken);
                Logger.LogDebug("Connected");
                break;

            }
            catch (SocketException socketException) when (socketException.ErrorCode == (int)SocketError.ConnectionRefused && tries <= 3)
            {
                Logger.LogDebug("Retrying... ({tries}/3)", tries);
                tries++;
                await Task.Delay(tries * 100);
            }
        }
        
        return client;
    }
}