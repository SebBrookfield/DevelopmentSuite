namespace Development.Suite.Ipc;

public interface IIpcClient : IIpcSender, IDisposable
{
    Task Start(CancellationToken cancellationToken);
    IEnumerable<IpcMessage> Messages { get; }
}