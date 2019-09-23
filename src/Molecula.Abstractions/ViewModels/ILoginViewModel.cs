using System.Collections.Generic;
using System.Windows.Input;
using Pamucuk.Mvvm.ViewModels;

namespace Molecula.Abstractions.ViewModels
{
    public interface ILoginViewModel : IViewModelBase
    {
        string CurrentFocusId { get; set; }

        string System { get; set; }

        string Language { get; set; }

        IEnumerable<string> AvailableSystems { get; }

        string User { get; set; }

        string Password { get; set; }

        ICommand LoginCommand { get; }

        ICommand SetPasswordCommand { get; }

        ICommand CancelLoginCommand { get; }
    }
}