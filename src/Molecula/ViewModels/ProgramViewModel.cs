using Molecula.Abstractions.ViewModels;
using Pamucuk.Mvvm.Observables;

namespace Molecula.ViewModels
{
    public class ProgramViewModel : ObservableObject, IProgramViewModel
    {
        public string ViewModelId { get; }

        public ProgramViewModel(string programId)
        {
            ViewModelId = programId;
        }
    }
}
