using System.Collections.Generic;
using System.Windows.Input;
using Molecula.Abstractions.Dtos;
using Molecula.Abstractions.Services;
using Molecula.Abstractions.ViewModels;
using Pamucuk.Mvvm.Commands;
using Pamucuk.Mvvm.Observables;

namespace Molecula.ViewModels
{
    public class MainMenuViewModel : ObservableObject, IMainMenuViewModel
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IProgramManager _programManager;
        public string ViewModelId => "MainMenu";

        private ICommand _startProgramCommand;
        public ICommand StartProgramCommand => _startProgramCommand ?? (_startProgramCommand = _commandFactory.Create<string>(_programManager.StartProgram));

        public IEnumerable<ProgramSetting> AvailablePrograms => _programManager.GetAvailablePrograms();

        public IEnumerable<ProgramSetting> QuickStartPrograms => _programManager.GetQuickStartPrograms();

        public MainMenuViewModel(
            ICommandFactory commandFactory,
            IProgramManager programManager)
        {
            _commandFactory = commandFactory;
            _programManager = programManager;
        }
    }
}