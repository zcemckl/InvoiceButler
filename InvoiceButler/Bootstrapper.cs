using Prism.Events;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace InvoiceButler
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var mainWindow = new MainWindow();

            Application.Current.MainWindow = mainWindow;
            return mainWindow;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IEventAggregator>(new EventAggregator());
        }
    }
}
