using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Data;
using Development.Suite.App.Common.ViewModels;
using Development.Suite.App.Models;
using Development.Suite.App.Plugin;
using Development.Suite.App.Utilities;
using Development.Suite.Logging;

namespace Development.Suite.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ICollectionView Commands { get; }

    public string? SearchTerm
    {
        get => _searchTerm;
        set
        {
            SetProperty(ref _searchTerm, value);
            Commands.Refresh();
        }
    }

    private readonly IDevelopmentSuiteLogger<MainViewModel> _logger;
    private readonly IpcClient _ipcClient;
    private string? _searchTerm;


    public MainViewModel(IDevelopmentSuiteLogger<MainViewModel> logger, IEnumerable<IPluginCommand> pluginCommands, IpcClient ipcClient)
    {
        _logger = logger;
        _ipcClient = ipcClient;
        _ipcClient.Start();

        Commands = CreateViewSource(pluginCommands.Select(c => new PluginCommand(c)), FilterCommands, p => p.Name, ListSortDirection.Ascending);
    }

    private static ICollectionView CreateViewSource<TItem>(IEnumerable<TItem> items, Func<TItem, bool> filter, Expression<Func<TItem, object>> property, ListSortDirection direction) where TItem : class
    {
        var propertyName = ((property.Body as MemberExpression)!.Member as PropertyInfo)!.Name;
        var viewSource = CollectionViewSource.GetDefaultView(items);
        viewSource.Filter = o => filter((TItem) o);
        viewSource.SortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Ascending));
        return viewSource;
    }

    private bool FilterCommands(PluginCommand pluginCommand)
    {
        var searchTerm = SearchTerm ?? string.Empty;
        return pluginCommand.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase);
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