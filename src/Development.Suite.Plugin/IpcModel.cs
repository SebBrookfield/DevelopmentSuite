namespace Development.Suite.Plugin;

public class IpcModel
{
    public Guid MessageId { get; }

    public IpcModel()
    {
        MessageId = Guid.NewGuid();
    }

    public IpcModel(IpcModel ipcModel)
    {
        MessageId = ipcModel.MessageId;
    }
}