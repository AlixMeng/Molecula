using Molecula.Abstractions.ViewModels;
using Pamucuk.Mvvm.Observables;

namespace Molecula.ViewModels
{
    public class ProgramViewModel : AbstractViewModel, IProgramViewModel
    {
        public override string ViewModelId { get; }

        public ObservableDictionary<string, object> Values { get; }

        public ProgramViewModel(string programId)
        {
            ViewModelId = programId;
            Values = new ObservableDictionary<string, object>(true);
        }
    }
}
