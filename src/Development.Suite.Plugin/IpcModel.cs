namespace Development.Suite.Plugin;

public class IpcModel
{
    public Guid Id { get; }

    public IpcModel()
    {
        Id = Guid.NewGuid();
    }

    public IpcModel(IpcModel ipcModel)
    {
        Id = ipcModel.Id;
    }
}