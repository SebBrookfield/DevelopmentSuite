namespace Development.Suite.Ipc.Common;

public interface IServiceMessageHandler<in TMessage> : IServiceMessageHandler, IMessageHandler<TMessage> where TMessage : IpcModel
{
}
public interface IClientMessageHandler<in TMessage> : IClientMessageHandler, IMessageHandler<TMessage> where TMessage : IpcModel
{
}

public interface IServiceMessageHandler : IMessageHandler
{
}

public interface IClientMessageHandler : IMessageHandler
{
}

public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : IpcModel
{
    Task HandleMessage(TMessage message);
}

public interface IMessageHandler
{
}