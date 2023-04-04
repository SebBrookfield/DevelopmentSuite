using Development.Suite.Logging;
using Development.Suite.Plugin;

namespace Development.Suite.Ipc.MessageHandling;

public class IpcMessageSender : IIpcMessageSender
{
    private readonly IDevelopmentSuiteLogger<IpcMessageSender> _logger;
    private readonly IIpcSender _ipcSender;

    public IpcMessageSender(IDevelopmentSuiteLogger<IpcMessageSender> logger, IIpcSender ipcSender)
    {
        _logger = logger;
        _ipcSender = ipcSender;
    }

    public void SendMessage<TMessage>(TMessage message) where TMessage : IpcModel
    {
        _logger.LogDebug("Sending message {@message}", message);
        _ipcSender.Send(IpcMessage.ToIpcMessage(message));
    }
}