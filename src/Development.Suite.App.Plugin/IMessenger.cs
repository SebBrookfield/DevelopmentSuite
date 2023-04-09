using Development.Suite.Ipc.Common;

namespace Development.Suite.App.Plugin;

public interface IMessenger
{
    Task<TReply> Send<TReply, TMessage>(TMessage message) where TReply : IpcModel where TMessage : IpcModel;

    Task<TReply> Send<TReply, TMessage>(TMessage message, CancellationToken cancellationToken)
        where TReply : IpcModel where TMessage : IpcModel;
    void ReceiveMessage(IpcModel? message);
}