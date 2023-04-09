using CommunityToolkit.Mvvm.Input;
using Development.Suite.App.Plugin;

namespace Development.Suite.App.Models;

public class PluginCommand
{
    public string Name { get; }
    public string Description { get; }
    public RelayCommand Command { get; }

    public PluginCommand(IPluginCommand pluginCommand)
    {
        Name = pluginCommand.Name;
        Description = pluginCommand.Description;
        Command = new RelayCommand(() => pluginCommand.Execute());
    }
}