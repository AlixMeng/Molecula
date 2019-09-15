using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Molecula.Abstractions.Services;
using Molecula.Abstractions.ViewModels;
using Molecula.Bootstrapping;
using Molecula.UI.Windows;
using Molecula.ViewModels;
using Pamucuk.Mvvm.Ioc;

namespace Molecula
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Bootstrapper _bootstrapper;
        private readonly ILocator _locator;

        public App()
        {
            _bootstrapper = new Bootstrapper();
            _locator = _bootstrapper.Locator;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainMenu = _locator.Get<IMainMenuViewModel>();
            var login = _locator.Get<ILoginViewModel>();
            var windowManager = _locator.Get<IWindowManager>();
            windowManager.OpenWindow(mainMenu);
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            if(windowManager.OpenDialog(login, mainMenu) != true)
                windowManager.CloseWindow(mainMenu);
        }
    }
}
