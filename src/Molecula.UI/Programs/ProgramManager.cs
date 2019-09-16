using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Molecula.Abstractions.Dtos;
using Molecula.Abstractions.Services;
using Pamucuk.Mvvm.ViewModels;

namespace Molecula.UI.Programs
{
    public class ProgramManager : IProgramManager
    {
        private readonly Func<string, IViewModelBase> _createProgramViewModel;
        private readonly IWindowManager _windowManager;
        private readonly List<ProgramSetting> _availablePrograms;

        public ProgramManager(
            Func<Func<string, IViewModelBase>> getProgramViewModelFactory,
            IWindowManager windowManager)
        {
            _createProgramViewModel = getProgramViewModelFactory();
            _windowManager = windowManager;
            _availablePrograms = new List<ProgramSetting>
            {
                new ProgramSetting{ProgramCategory = "Orders", ProgramId = "Orders", IsInQuickStart = true},
                new ProgramSetting{ProgramCategory = "Samples", ProgramId = "Samples", IsInQuickStart = true},
                new ProgramSetting{ProgramCategory = "Results", ProgramId = "ResultsBySample", IsInQuickStart = true},
                new ProgramSetting{ProgramCategory = "Results", ProgramId = "ResultsByMethod", IsInQuickStart = true},
            };
        }

        public IEnumerable<ProgramSetting> GetAvailablePrograms()
        {
            return _availablePrograms;
        }

        public IEnumerable<ProgramSetting> GetQuickStartPrograms()
        {
            var quickstartPrograms = _availablePrograms.Where(setting => setting.IsInQuickStart).ToArray();
            foreach(var program in quickstartPrograms)
                EnsureProgramResources(program.ProgramId);
            return quickstartPrograms;
        }

        public void StartProgram(string program)
        {
            EnsureProgramResources(program);
            var programViewModel = _createProgramViewModel(program);
            _windowManager.OpenWindow(programViewModel);
        }

        private void EnsureProgramResources(string program)
        {
            var resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Programs", $"{program}.xaml");
            if (!File.Exists(resourcePath))
                return;

            var programUri = new Uri($"file:///{resourcePath}");
            if (IsProgramResourceLoaded(programUri))
                return;

            var resourceDictionary = new ResourceDictionary {Source = programUri };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private bool IsProgramResourceLoaded(Uri programUri)
            => Application.Current.Resources.MergedDictionaries.Any(dictionary => programUri.Equals(dictionary.Source));
    }
}
