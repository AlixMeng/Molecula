using System;
using System.Collections.ObjectModel;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.ViewModels;
using Molecula.Workflows.Designer.Nodes;

namespace Molecula.ViewModels
{
    public class WorkflowDesignerViewModel : AbstractViewModel, IWorkflowDesignerViewModel
    {
        public override string ViewModelId => "WorkflowDesigner";

        private ObservableCollection<IWorkspace> _workspaces;
        public ObservableCollection<IWorkspace> Workspaces => _workspaces ??= new ObservableCollection<IWorkspace>();

        private IWorkspace _currentWorkspace;
        public IWorkspace CurrentWorkspace
        {
            get => _currentWorkspace;
            set => Set(ref _currentWorkspace, value);
        }

        private IWorkflowItemToolbox _workflowItemToolbox;
        public IWorkflowItemToolbox WorkflowItemToolbox
        {
            get => _workflowItemToolbox;
            set => Set(ref _workflowItemToolbox, value);
        }

        public WorkflowDesignerViewModel(Func<IWorkspace> workspaceFactory, IWorkflowItemToolbox toolbox)
        {
            var workspace = workspaceFactory();
            workspace.Name = "Test";
            workspace.AddNode(typeof(StartNode));
            Workspaces.Add(workspace);
            CurrentWorkspace = workspace;
            WorkflowItemToolbox = toolbox;
        }
    }
}