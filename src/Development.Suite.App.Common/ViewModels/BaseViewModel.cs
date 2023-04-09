using CommunityToolkit.Mvvm.ComponentModel;

namespace Development.Suite.App.Common.ViewModels;

public class BaseViewModel : ObservableObject
{
    public bool Loaded { get; set; }

    public bool Loading
    {
        get => _loading;
        set => SetProperty(ref _loading, value);
    }

    private bool _loading;

    public virtual Task Load()
    {
        Loaded = true;
        return Task.CompletedTask;
    }
}