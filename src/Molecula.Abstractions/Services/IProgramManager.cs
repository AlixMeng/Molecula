using System.Collections.Generic;
using Molecula.Abstractions.Dtos;

namespace Molecula.Abstractions.Services
{
    public interface IProgramManager
    {
        void StartProgram(string program);

        IEnumerable<ProgramSetting> GetAvailablePrograms();

        IEnumerable<ProgramSetting> GetQuickStartPrograms();
    }
}