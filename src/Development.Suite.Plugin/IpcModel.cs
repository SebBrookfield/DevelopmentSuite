namespace Development.Suite.Ipc.Common;

[Serializable]
public class IpcModel
{
    public Guid MessageId { get; set; }

    public IpcModel()
    {
        MessageId = Guid.NewGuid();
    }

    public IpcModel(IpcModel ipcModel)
    {
        MessageId = ipcModel.MessageId;
    }
}