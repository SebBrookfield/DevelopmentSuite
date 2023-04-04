using Development.Suite.Ipc;
using Development.Suite.Logging;
using Development.Suite.Plugin;

namespace Development.Suite.Service;

public class MessageSender : IMessageSender
{
    private readonly IIpcServer _ipcServer;
    private readonly IDevelopmentSuiteLogger<MessageSender> _logger;

    public MessageSender(IIpcServer ipcServer, IDevelopmentSuiteLogger<MessageSender> logger)
    {
        _ipcServer = ipcServer;
        _logger = logger;
    }

    public void SendMessage<TMessage>(TMessage message) where TMessage : class
    {
        _logger.LogDebug("Sending message {@message}", message);
        _ipcServer.Send(IpcMessage.ToIpcMessage(message));
    }
}