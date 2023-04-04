namespace Development.Suite.Plugin;

public interface IIpcMessageSender
{
    void SendMessage<TMessage>(TMessage message) where TMessage : IpcModel;
}