using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Development.Suite.App.Common.ViewModels;
using Development.Suite.App.Models;
using Development.Suite.App.Plugin;
using Development.Suite.App.Utilities;
using Development.Suite.Logging;

namespace Development.Suite.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ObservableCollection<PluginCommand> Commands { get; }

    public string? SearchTerm
    {
        get => _searchTerm;
        set => SetProperty(ref _searchTerm, value);
    }

    private readonly IDevelopmentSuiteLogger<MainViewModel> _logger;
    private readonly IpcClient _ipcClient;
    private string? _searchTerm;


    public MainViewModel(IDevelopmentSuiteLogger<MainViewModel> logger, IEnumerable<IPluginCommand> pluginCommands, IpcClient ipcClient)
    {
        _logger = logger;
        _ipcClient = ipcClient;
        Commands = new ObservableCollection<PluginCommand>(pluginCommands.Select(c => new PluginCommand(c)));

        _ipcClient.Start();
    }

    public void OnSetToBackground()
    {
        SearchTerm = null;
    }

    public void OnClose()
    {
        _ipcClient.Stop();
    }
}