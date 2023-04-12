using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Development.Suite.App.Common.ViewModels;
using Development.Suite.App.Plugin;
using Development.Suite.App.Utilities;
using Development.Suite.App.Views;
using Development.Suite.Common.ExtensionMethods;
using Development.Suite.Logging;

namespace Development.Suite.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    public RelayCommand<IPluginCommand> RunCommand { get; set; }
    public RelayCommand CloseCommand { get; set; }
    public RelayCommand SelectFirstCommand { get; set; }
    public RelayCommand ShowAllCommand { get; set; }

    public List<IPluginCommand> Commands
    {
        get => _commands;
        set => SetProperty(ref _commands, value);
    }

    public string? SearchTerm
    {
        get => _searchTerm;
        set
        {
            SetProperty(ref _searchTerm, value);
            FilterCommands();
        }
    }

    private readonly IDevelopmentSuiteLogger<MainViewModel> _logger;
    private readonly IpcClient _ipcClient;
    private readonly List<IPluginCommand> _originalCommands;
    private string? _searchTerm;
    private List<IPluginCommand> _commands;

    public MainViewModel(IDevelopmentSuiteLogger<MainViewModel> logger, IEnumerable<IPluginCommand> pluginCommands, IpcClient ipcClient)
    {
        _logger = logger;
        _ipcClient = ipcClient;
        
        if (!DesignMode)
            _ipcClient.Start();

        Commands = new List<IPluginCommand>();

        _originalCommands = pluginCommands
            .OrderBy(c => c.Name)
            .ToList();

        RunCommand = new RelayCommand<IPluginCommand>(Run);
        CloseCommand = new RelayCommand(SetToBackground);
        SelectFirstCommand = new RelayCommand(() => Run(Commands.FirstOrDefault()));
        ShowAllCommand = new RelayCommand(() =>
        {
            Commands = _originalCommands;
        });
    }

    private async void Run(IPluginCommand? pluginCommand)
    {
        if (pluginCommand == null)
            return;

        _logger.LogDebug($"Executing command {pluginCommand}");

        try
        {
            await pluginCommand.Execute();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Failed to execute command {pluginCommand.Name}.");
        }
        finally
        {
            SetToBackground();
        }
    }

    private void FilterCommands()
    {
        Commands = SearchTerm.IsNullOrWhitespace()
            ? new List<IPluginCommand>()
            : _originalCommands.Where(c => c.Name.Contains(SearchTerm!, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private static void SetToBackground()
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
            mainWindow.SetToBackground();
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