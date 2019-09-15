using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Molecula.Abstractions.ViewModels;
using Molecula.ViewModels;

namespace Molecula.Resources
{
	public partial class Login
	{
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && passwordBox.DataContext is ILoginViewModel vm)
            {
                vm.Password = passwordBox.Password;
            }
        }

        private void OnPasswordPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
                && sender is PasswordBox passwordBox
                && passwordBox.DataContext is ILoginViewModel vm
                && vm.LoginCommand.CanExecute(null))
            {
                vm.LoginCommand.Execute(null);
            }
        }
    }
}
		