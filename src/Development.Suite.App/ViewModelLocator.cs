using CommunityToolkit.Mvvm.DependencyInjection;
using Development.Suite.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Development.Suite.App
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddTransient<MainViewModel>()
                    .BuildServiceProvider());
        }

        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();
    }
}