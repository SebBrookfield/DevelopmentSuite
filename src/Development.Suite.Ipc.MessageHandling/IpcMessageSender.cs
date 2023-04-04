using Development.Suite.Logging;
using Development.Suite.Plugin;

namespace Development.Suite.Ipc.MessageHandling;

public class IpcMessageSender : IIpcMessageSender
{
    private readonly IIpcServer _ipcServer;
    private readonly IDevelopmentSuiteLogger<IpcMessageSender> _logger;

    public IpcMessageSender(IIpcServer ipcServer, IDevelopmentSuiteLogger<IpcMessageSender> logger)
    {
        _ipcServer = ipcServer;
        _logger = logger;
    }

    public void SendMessage<TMessage>(TMessage message) where TMessage : IpcModel
    {
        _logger.LogDebug("Sending message {@message}", message);
        _ipcServer.Send(IpcMessage.ToIpcMessage(message));
    }
}