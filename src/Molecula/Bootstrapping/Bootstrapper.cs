using Molecula.ViewModels;
using Molecula.Views;
using Pamucuk.Mvvm.Ioc;

namespace Molecula.Bootstrapping
{
    public class Bootstrapper
    {
        public ILocator Locator { get; private set; }

        private readonly IIocContainer _ioc;

        public Bootstrapper()
        {
            _ioc = new IocContainer();

            RegisterServices();
            RegisterViewModels();
            RegisterViews();

            Locator = new Locator(_ioc);
        }

        public void RegisterServices()
        {

        }

        public void RegisterViewModels()
        {
            _ioc.Register<LoginViewModel, ILoginViewModel>();
            _ioc.Register<MainMenuViewModel, IMainMenuViewModel>();
        }

        public void RegisterViews()
        {
            _ioc.Register<LoginView>(() =>
            {
                var login = new LoginView();
                LocatorExtension.SetLocator(login, Locator);
                return login;
            });

            _ioc.Register<MainMenuView>(() =>
            {
                var mainMenu = new MainMenuView();
                LocatorExtension.SetLocator(mainMenu, Locator);
                return mainMenu;
            });
        }

    }
}
