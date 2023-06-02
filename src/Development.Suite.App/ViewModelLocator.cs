using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Development.Suite.App.Common.ViewModels;

namespace Development.Suite.App
{
    public class ViewModelLocator
    {
        private readonly IContainer _container;

        public ViewModelLocator()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AutofacModule>();
            _container = containerBuilder
                .Build();
        }

        public BaseViewModel this[string viewModelName]
        {
            get
            {
                var viewModel = _container.ResolveKeyed<BaseViewModel>(viewModelName);
                
                if (!viewModel.IsLoaded)
                {
                    var task = Application.Current.Dispatcher.Invoke<Task>(async () =>
                    {
                        await viewModel.Load();
                        viewModel.IsLoaded = true;
                    }, DispatcherPriority.Background);
                }
                
                return viewModel;
            }
        }
    }
}