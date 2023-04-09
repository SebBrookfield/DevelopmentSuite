namespace Development.Suite.Ipc.Common;

public interface IMessageHandler<in TMessage> where TMessage : IpcModel
{
    Task HandleMessage(TMessage message);
}