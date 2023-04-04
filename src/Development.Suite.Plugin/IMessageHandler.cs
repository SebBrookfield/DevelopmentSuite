namespace Development.Suite.Plugin;

public interface IMessageHandler<in TMessage> where TMessage : IpcModel
{
    void HandleMessage(TMessage message);
}