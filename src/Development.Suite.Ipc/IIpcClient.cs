namespace Development.Suite.Ipc;

public interface IIpcClient : IIpcSender, IDisposable
{
    Task Start(CancellationToken cancellationToken);
    IAsyncEnumerable<IpcMessage> Messages { get; }
}