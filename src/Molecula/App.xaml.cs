using System.Windows;
using Molecula.Bootstrapping;
using Molecula.Views;
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
            var mainMenu = _locator.Get<MainMenuView>();
            var login = _locator.Get<LoginView>();
            mainMenu.Show();
            if(login.ShowDialog() != true)
                mainMenu.Close();
        }
    }
}
