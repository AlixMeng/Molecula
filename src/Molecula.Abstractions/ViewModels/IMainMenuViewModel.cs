using System.Collections.Generic;
using System.Windows.Input;
using Molecula.Abstractions.Dtos;
using Pamucuk.Mvvm.ViewModels;

namespace Molecula.Abstractions.ViewModels
{
    public interface IMainMenuViewModel : IAbstractViewModel
    {
        ICommand StartProgramCommand { get; }

        IEnumerable<ProgramSetting> AvailablePrograms { get; }

        IEnumerable<ProgramSetting> QuickStartPrograms { get; }
    }
}