namespace Development.Suite.App.Plugin;

public interface IPluginCommand
{
    string Name { get; }
    string Description { get; }
    Task Execute();
}