namespace Development.Suite.Ipc;

public interface IIpcServer : IIpcSender, IDisposable
{
    Task Start(CancellationToken cancellationToken);
    IAsyncEnumerable<IpcMessage> Messages { get; }
}