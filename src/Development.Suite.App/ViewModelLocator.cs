using Autofac;
using Development.Suite.App.ViewModels;

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

        public MainViewModel Main => _container.Resolve<MainViewModel>();
    }
}