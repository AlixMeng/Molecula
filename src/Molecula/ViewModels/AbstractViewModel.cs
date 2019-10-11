using Molecula.Abstractions.ViewModels;
using Pamucuk.Mvvm.Observables;

namespace Molecula.ViewModels
{
    public abstract class AbstractViewModel : ObservableObject, IAbstractViewModel
    {
        public abstract string ViewModelId { get; }

        private string _currentFocusId;
        public string CurrentFocusId => _currentFocusId;

        public void SetCurrentFocusId(string focusId)
        {
            Set(ref _currentFocusId, null, nameof(CurrentFocusId));
            Set(ref _currentFocusId, focusId, nameof(CurrentFocusId));
        }
    }
}
