namespace Development.Suite.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    private string? _searchTerm;
    
    public string? SearchTerm
    {
        get => _searchTerm;
        set => SetProperty(ref _searchTerm, value);
    }

    public void OnSetToBackground()
    {
        SearchTerm = null;
    }
}