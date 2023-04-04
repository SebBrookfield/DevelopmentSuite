using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Development.Suite.Logging;

namespace Development.Suite.Ipc.Tcp;

public abstract class TcpIpcBase
{
    protected readonly TcpIpcConfig TcpIpcConfig;
    protected CancellationToken CancellationToken;
    protected readonly IDevelopmentSuiteLogger<TcpIpcBase> Logger;

    private TcpClient? _client;
    private NetworkStream? _stream;
    private BinaryWriter? _binaryWriter;
    private BinaryReader? _binaryReader;
    private bool _active;

    private protected TcpIpcBase(TcpIpcConfig tcpIpcConfig, IDevelopmentSuiteLogger<TcpIpcBase> logger)
    {
        TcpIpcConfig = tcpIpcConfig;
        Logger = logger;
    }

    protected abstract Task<TcpClient> GetTcpClient();

    public virtual async Task Start(CancellationToken cancellationToken)
    {
        if (_active)
        {
            Logger.LogWarning("Already started");
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        CancellationToken = cancellationToken;
        _client = await GetTcpClient();
        _stream = _client.GetStream();
        _binaryWriter = new BinaryWriter(_stream);
        _binaryReader = new BinaryReader(_stream);
        _active = true;
    }

    public IEnumerable<IpcMessage> Messages => ReadMessages();

    private IEnumerable<IpcMessage> ReadMessages()
    {
        if (!_active)
            throw new TcpIpcNotStartedException();

        var bytes = new List<byte>(4096);

        while (!CancellationToken.IsCancellationRequested)
        {
            byte readByte;

            try
            {
                readByte = _binaryReader!.ReadByte();
            }
            catch (IOException)
            {

                Restart();
                continue;
            }

            if (readByte == 0x04)
            {
                var messageString = Encoding.Default.GetString(bytes.ToArray()).Replace("\0", string.Empty).Trim();
                yield return JsonSerializer.Deserialize<IpcMessage>(messageString)!;
                bytes.Clear();
            }
            else
            {
                bytes.Add(readByte);
            }

        }
    }

    private void Restart()
    {
        Logger.LogInformation("Restarting");
        Dispose();
        _active = false;
        Start(CancellationToken).GetAwaiter().GetResult();
    }

    public void Send(IpcMessage message)
    {
        if (!_active)
            throw new TcpIpcNotStartedException();

        Logger.LogDebug("Writing message bytes");
        foreach (var @byte in Encoding.Default.GetBytes(JsonSerializer.Serialize(message)))
            _binaryWriter!.Write(@byte);

        Logger.LogDebug("Flushing stream");
        _binaryWriter!.Write(0x04);
        _binaryWriter.Flush();
        Logger.LogDebug("Flushed");
    }

    public void Dispose()
    {
        _binaryWriter?.Dispose();
        _binaryReader?.Dispose();
        _stream?.Dispose();
        _client?.Dispose();
    }
}