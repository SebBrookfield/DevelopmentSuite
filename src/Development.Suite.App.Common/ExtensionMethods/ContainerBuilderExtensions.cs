using Autofac;
using Development.Suite.App.Common.ViewModels;

namespace Development.Suite.App.Common.ExtensionMethods;

public static class ContainerBuilderExtensions
{
    public static void RegisterViewModel<TViewModel>(this ContainerBuilder containerBuilder) where TViewModel : BaseViewModel
    {
        containerBuilder.RegisterType<TViewModel>().Keyed<BaseViewModel>(typeof(TViewModel).Name);
    }
}