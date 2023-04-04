namespace Development.Suite.Ipc;

public interface IIpcServer : IIpcSender, IDisposable
{
    Task Start(CancellationToken cancellationToken);
    IEnumerable<IpcMessage> Messages { get; }
}