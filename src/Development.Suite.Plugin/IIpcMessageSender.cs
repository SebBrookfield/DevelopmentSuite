namespace Development.Suite.Ipc.Common;

public interface IIpcMessageSender
{
    Task SendMessage<TMessage>(TMessage message) where TMessage : IpcModel;
}