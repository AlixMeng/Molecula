using System.Collections.ObjectModel;
using Molecula.Abstractions.ViewModels;
using Molecula.Abstractions.Workflows.Core;

namespace Molecula.Abstractions.Workflows.ViewModels
{
    public interface IWorkflowDesignerViewModel : IAbstractViewModel
    {
        ObservableCollection<IWorkspace> Workspaces { get; }

        IWorkspace CurrentWorkspace { get; set; }
    }
}