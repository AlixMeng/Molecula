using System;
using System.Windows;
using Molecula.Abstractions.Services;
using Molecula.Abstractions.ViewModels;
using Molecula.UI.Programs;
using Molecula.UI.Windows;
using Molecula.ViewModels;
using Pamucuk.Mvvm.Commands;
using Pamucuk.Mvvm.Ioc;
using Pamucuk.Mvvm.ViewModels;
using Pamucuk.UI.Controls.Windows;

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
            _ioc.Register(() => Locator);
        }

        public void RegisterServices()
        {
            _ioc.Register<RelayCommandFactory, ICommandFactory>();
            _ioc.Register<WindowManager, IWindowManager>();
            _ioc.Register<ProgramManager, IProgramManager>();
        }

        public void RegisterViewModels()
        {
            _ioc.Register<LoginViewModel, ILoginViewModel>();
            _ioc.Register<MainMenuViewModel, IMainMenuViewModel>();
            _ioc.Register<Func<string, IViewModelBase>>(
                () =>
                    programId =>
                    {
                        var vm = new ProgramViewModel(programId);
                        return vm;
                    });
        }

        public void RegisterViews()
        {
            _ioc.Register<Window>(() =>
            {
                var window = new AppWindow();
                LocatorExtension.SetLocator(window, Locator);
                return window;
            });
        }
    }
}
