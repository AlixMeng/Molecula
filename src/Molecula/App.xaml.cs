using System.Windows;
using Molecula.Abstractions.Services;
using Molecula.Abstractions.ViewModels;
using Molecula.Bootstrapping;
using Pamucuk.Mvvm.Ioc;

namespace Molecula
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly ILocator _locator;

        public App()
        {
            var bootstrapper = new Bootstrapper();
            _locator = bootstrapper.Locator;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainMenu = _locator.Get<IMainMenuViewModel>();
            var login = _locator.Get<ILoginViewModel>();
            var windowManager = _locator.Get<IWindowManager>();
            windowManager.OpenWindow(mainMenu);
            ShutdownMode = ShutdownMode.OnLastWindowClose;
            if(windowManager.OpenDialog(login, mainMenu) != true)
                windowManager.CloseWindow(mainMenu);
        }
    }
}
