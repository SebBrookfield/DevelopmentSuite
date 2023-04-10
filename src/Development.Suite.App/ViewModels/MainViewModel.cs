using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using Development.Suite.App.Common.ViewModels;
using Development.Suite.App.Models;
using Development.Suite.App.Plugin;
using Development.Suite.App.Utilities;
using Development.Suite.App.Views;
using Development.Suite.Common.ExtensionMethods;
using Development.Suite.Logging;

namespace Development.Suite.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    public RelayCommand CloseCommand { get; set; }
    public RelayCommand? SelectFirstCommand { get; set; }
    public RelayCommand? ShowAllCommand { get; set; }

    public List<PluginCommand> Commands
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
    private string? _searchTerm;
    private List<PluginCommand> _originalCommands;
    private List<PluginCommand> _commands;

    public MainViewModel(IDevelopmentSuiteLogger<MainViewModel> logger, IEnumerable<IPluginCommand> pluginCommands, IpcClient ipcClient)
    {
        _logger = logger;
        _ipcClient = ipcClient;
        
        //if (!DesignMode)
          //  _ipcClient.Start();

          Commands = new List<PluginCommand>();
            
        _originalCommands = pluginCommands
            .Select(c => new PluginCommand(c))
            .OrderBy(c => c.Name)
            .ToList();

        CloseCommand = new RelayCommand(SetToBackground);
        SelectFirstCommand = Commands.FirstOrDefault()?.Command;
        ShowAllCommand = new RelayCommand(() =>
        {
            Commands = _originalCommands;
        });
    }
    //
    // private static ICollectionView CreateViewSource<TItem>(IEnumerable<TItem> items, Func<TItem, bool> filter, Expression<Func<TItem, object>> property, ListSortDirection direction) where TItem : class
    // {
    //     var propertyName = ((property.Body as MemberExpression)!.Member as PropertyInfo)!.Name;
    //     var viewSource = CollectionViewSource.GetDefaultView(items);
    //     viewSource.Filter = o => filter((TItem) o);
    //     viewSource.SortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Ascending));
    //     return viewSource;
    // }

    private void FilterCommands()
    {
        Commands = SearchTerm.IsNullOrWhitespace()
            ? new List<PluginCommand>()
            : _originalCommands.Where(c => c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
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