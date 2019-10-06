using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Molecula.Abstractions;
using Molecula.Abstractions.Dtos;
using Molecula.Abstractions.Services;
using YamlDotNet.Serialization;

namespace Molecula.UI.Programs
{
    public class ProgramManager : IProgramManager
    {
        private readonly CreateProgramViewModel _createProgramViewModel;
        private readonly IWindowManager _windowManager;
        private IEnumerable<ProgramSetting> _availablePrograms;
        private const string ProgramTemplateName = "ProgramTemplate";

        public ProgramManager(
            Func<CreateProgramViewModel> getCreateProgramViewModelFactory,
            IWindowManager windowManager)
        {
            _createProgramViewModel = getCreateProgramViewModelFactory();
            _windowManager = windowManager;
        }

        public IEnumerable<ProgramSetting> GetAvailablePrograms()
        {
            return _availablePrograms ??= LoadPrograms();
        }

        private static IEnumerable<ProgramSetting> LoadPrograms()
        {
            var programsContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Programs.yaml"));
            var deserializer = new DeserializerBuilder().Build();
            var programs = deserializer.Deserialize<ProgramSetting[]>(programsContent);
            return programs;
        }

        public IEnumerable<ProgramSetting> GetQuickStartPrograms()
        {
            var quickstartPrograms = GetAvailablePrograms().Where(IsInQuickstart).ToArray();
            foreach(var program in quickstartPrograms)
                EnsureProgramResources(program.Id);
            return quickstartPrograms;
        }

        private static bool IsInQuickstart(ProgramSetting program)
        {
            return true;
        }

        public void StartProgram(string program)
        {
            EnsureProgramResources(program);
            var programViewModel = _createProgramViewModel(program);
            _windowManager.OpenWindow(programViewModel);
        }

        private static void EnsureProgramResources(string program)
        {
            var resourcePath = GetProgramPath(program);
            if (!ExistsProgram(resourcePath))
            {
                CreateProgramFromTemplate(program, resourcePath);
            }

            var programUri = new Uri($"file:///{resourcePath}");
            if (IsProgramResourceLoaded(programUri))
                return;

            var resourceDictionary = new ResourceDictionary {Source = programUri };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private static void CreateProgramFromTemplate(string program, string resourcePath)
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", $"{ProgramTemplateName}.xaml");
            var content = File.ReadAllText(templatePath).Replace($"${ProgramTemplateName}$", program);
            File.WriteAllText(resourcePath, content);
        }

        private static bool ExistsProgram(string resourcePath)
            => File.Exists(resourcePath);

        private static string GetProgramPath(string program)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Programs", $"{program}.xaml");

        private static bool IsProgramResourceLoaded(Uri programUri)
            => Application.Current.Resources.MergedDictionaries.Any(dictionary => programUri.Equals(dictionary.Source));
    }
}
