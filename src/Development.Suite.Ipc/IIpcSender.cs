namespace Development.Suite.Ipc;

public interface IIpcSender
{
    void Send(IpcMessage message);
}