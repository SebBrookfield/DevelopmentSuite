namespace Development.Suite.Plugin;

public interface IIpcMessageSender
{
    Task SendMessage<TMessage>(TMessage message) where TMessage : IpcModel;
}