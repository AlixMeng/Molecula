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

        private string _currentFocusId;
        public string CurrentFocusId
        {
            get => _currentFocusId;
            set
            {
                Set(ref _currentFocusId, null);
                Set(ref _currentFocusId, value);
            }
        }

        private string _system;
        public string System
        {
            get => _system;
            set
            {
                if (Set(ref _system, value))
                {
                    Settings.Default.LastSelectedSystem = _system;
                }
            }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (Set(ref _language, value))
                {
                    Settings.Default.LastSelectedLanguage = _language;
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
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = _commandFactory.Create<bool?>(Login, CanLogin));

        private ICommand _setPasswordCommand;
        public ICommand SetPasswordCommand => _setPasswordCommand ?? (_setPasswordCommand = _commandFactory.Create<string>(SetPassword));

        private ICommand _cancelLoginCommand;
        public ICommand CancelLoginCommand => _cancelLoginCommand ?? (_cancelLoginCommand = _commandFactory.Create<object>(CancelLogin));

        public LoginViewModel(
            ICommandFactory commandFactory,
            IWindowManager windowManager)
        {
            _commandFactory = commandFactory;
            _windowManager = windowManager;
            System = Settings.Default.LastSelectedSystem;
            Language = Settings.Default.LastSelectedLanguage;
            CurrentFocusId = nameof(User);
        }

        private static IEnumerable<string> FillAvailableSystems()
            => Settings.Default.Properties
                .OfType<SettingsProperty>()
                .Where(property => property.Name.StartsWith("AvailableSystem"))
                .OrderBy(property => property.Name)
                .Select(property => Settings.Default[property.Name]?.ToString())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .ToList();

        private bool CanLogin(bool? parameter)
        {
            return !string.IsNullOrWhiteSpace(System)
                   && !string.IsNullOrWhiteSpace(User)
                   && !string.IsNullOrWhiteSpace(Password)
                   && parameter == true;
        }

        private void Login(bool? parameter)
        {
            Settings.Default.Save();
            _windowManager.CloseDialog(this, true);
        }

        private void CancelLogin(object obj)
        {
            _windowManager.CloseDialog(this, false);
        }

        private void SetPassword(string password)
        {
            Password = password;
        }

    }
}
