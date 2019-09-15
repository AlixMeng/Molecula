using Molecula.Abstractions.ViewModels;
using Pamucuk.Mvvm.Observables;

namespace Molecula.ViewModels
{
    public delegate IProgramViewModel CreateProgramViewModel(string viewModelId);

    public class ProgramViewModel : ObservableObject, IProgramViewModel
    {
        public string ViewModelId { get; }

        public ProgramViewModel(string programId)
        {
            ViewModelId = programId;
        }
    }
}
