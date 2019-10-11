using Pamucuk.Mvvm.Observables;

namespace Molecula.Abstractions.ViewModels
{
    public interface IProgramViewModel : IAbstractViewModel
    {
        ObservableDictionary<string, object> Values { get; }
    }
}