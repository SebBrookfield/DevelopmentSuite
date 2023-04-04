namespace Development.Suite.Ipc;

public interface IIpcClient : IDisposable
{
    Task Start(CancellationToken cancellationToken);
    void Send(IpcMessage message);
    IEnumerable<IpcMessage> Messages { get; }
}