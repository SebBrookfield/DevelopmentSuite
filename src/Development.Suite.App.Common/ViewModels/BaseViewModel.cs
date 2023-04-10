using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows;

namespace Development.Suite.App.Common.ViewModels;

public class BaseViewModel : ObservableObject
{
    public bool Loaded { get; set; }

    public bool Loading
    {
        get => _loading;
        set => SetProperty(ref _loading, value);
    }

    public bool DesignMode => (bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

    private bool _loading;

    public virtual Task Load()
    {
        Loaded = true;
        return Task.CompletedTask;
    }
}