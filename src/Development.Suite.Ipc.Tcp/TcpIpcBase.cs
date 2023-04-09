using System.Net.Sockets;
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
    private StreamWriter? _streamWriter;
    private StreamReader? _streamReader;
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
        _streamWriter = new StreamWriter(_stream);
        _streamReader = new StreamReader(_stream);
        _active = true;
    }

    public IAsyncEnumerable<IpcMessage> Messages => ReadMessages();

    private async IAsyncEnumerable<IpcMessage> ReadMessages()
    {
        if (!_active)
            throw new TcpIpcNotStartedException();

        while (!CancellationToken.IsCancellationRequested)
        {
            Logger.LogDebug("Reading stream");
            var streamedString = await ReadStream();
            Logger.LogDebug("Parsing message {@streamedString}", streamedString ?? "");
            var message = ParseMessage(streamedString);

            if (message != null)
                yield return message;
        }
    }

    private IpcMessage? ParseMessage(string? message)
    {
        var json = message?.Trim();

        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<IpcMessage>(json)!;
        }
        catch(Exception exception)
        {
            Logger.LogError(exception, "Failed to deserialize json. {@json}", json);
            return null;
        }
    }

    private async Task<string?> ReadStream()
    {
        try
        {
            return await _streamReader!.ReadLineAsync(CancellationToken);
        }
        catch (IOException)
        {
            await Restart();
            return null;
        }
    }

    private async Task Restart()
    {
        Logger.LogInformation("Restarting");
        Dispose();
        _active = false;
        await Start(CancellationToken);
    }

    public async Task Send(IpcMessage message)
    {
        if (!_active)
            throw new TcpIpcNotStartedException();

        Logger.LogDebug("Serializing message");
        var json = JsonSerializer.Serialize(message);
        Logger.LogDebug("Writing to stream");
        await _streamWriter!.WriteLineAsync(json);
        Logger.LogDebug("Flushing stream");
        await _streamWriter.FlushAsync();
        Logger.LogDebug("Flushed");
    }

    public void Dispose()
    {
        _streamWriter?.Dispose();
        _streamReader?.Dispose();
        _stream?.Dispose();
        _client?.Dispose();
    }
}