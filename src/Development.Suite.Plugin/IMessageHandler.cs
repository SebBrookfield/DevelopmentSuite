namespace Development.Suite.Plugin;

public interface IMessageHandler<in TMessage> where TMessage : class
{
    void HandleMessage(TMessage message);
}