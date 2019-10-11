using Pamucuk.Mvvm.ViewModels;

namespace Molecula.Abstractions.ViewModels
{
    public interface IAbstractViewModel : IViewModelBase
    {
        string CurrentFocusId { get; }

        void SetCurrentFocusId(string focusId);
    }
}
