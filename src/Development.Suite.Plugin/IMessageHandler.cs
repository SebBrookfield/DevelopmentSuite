namespace Development.Suite.Plugin;

public interface IMessageHandler<in TMessage> where TMessage : IpcModel
{
    Task HandleMessage(TMessage message);
}