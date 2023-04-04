namespace Development.Suite.Plugin;

public interface IMessageSender
{
    void SendMessage<TMessage>(TMessage message) where TMessage : class;
}