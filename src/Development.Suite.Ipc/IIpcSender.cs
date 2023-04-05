namespace Development.Suite.Ipc;

public interface IIpcSender
{
    Task Send(IpcMessage message);
}