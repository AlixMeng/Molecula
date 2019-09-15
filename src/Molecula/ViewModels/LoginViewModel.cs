using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using Molecula.Abstractions.Services;
using Molecula.Abstractions.ViewModels;
using Molecula.Properties;
using Pamucuk.Mvvm.Commands;
using Pamucuk.Mvvm.Observables;

namespace Molecula.ViewModels
{
    public class LoginViewModel : ObservableObject, ILoginViewModel
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IWindowManager _windowManager;

        public string ViewModelId => "Login";

        private string _system;
        public string System
        {
            get => _system;
            set
            {
                if (Set(ref _system, value))
                {
                    UpdateSystem(_system);
                }
            }
        }

        private IEnumerable<string> _availableSystems;
        public IEnumerable<string> AvailableSystems => _availableSystems ?? (_availableSystems = FillAvailableSystems());

        private string _user;
        public string User
        {
            get => _user;
            set => Set(ref _user, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private ICommand _loginCommand;
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = _commandFactory.Create<object>(Login, CanLogin));

        private ICommand _cancelLoginCommand;
        public ICommand CancelLoginCommand => _cancelLoginCommand ?? (_cancelLoginCommand = _commandFactory.Create<object>(CancelLogin));

        public LoginViewModel(
            ICommandFactory commandFactory,
            IWindowManager windowManager)
        {
            _commandFactory = commandFactory;
            _windowManager = windowManager;
            System = Settings.Default.LastSelectedSystem;
        }

        private static IEnumerable<string> FillAvailableSystems()
            => Settings.Default.Properties
                .OfType<SettingsProperty>()
                .Where(property => property.Name.StartsWith("AvailableSystem"))
                .OrderBy(property => property.Name)
                .Select(property => Settings.Default[property.Name]?.ToString())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .ToList();

        private void UpdateSystem(string system)
        {
            Settings.Default.LastSelectedSystem = system;
            Settings.Default.Save();
        }

        private bool CanLogin(object arg)
        {
            return !string.IsNullOrWhiteSpace(System)
                   && !string.IsNullOrWhiteSpace(User)
                   && !string.IsNullOrWhiteSpace(Password);
        }

        private void Login(object obj)
        {
            _windowManager.CloseDialog(this, true);
        }

        private void CancelLogin(object obj)
        {
            _windowManager.CloseDialog(this, false);
        }
    }
}
