using System;
using System.Windows;
using Molecula.Abstractions;
using Molecula.Abstractions.Services;
using Molecula.Abstractions.ViewModels;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Molecula.Abstractions.Workflows.ViewModels;
using Molecula.UI.Programs;
using Molecula.UI.Windows;
using Molecula.ViewModels;
using Molecula.Workflows.Designer.Core;
using Molecula.Workflows.Designer.Nodes;
using Pamucuk.Mvvm.Commands;
using Pamucuk.Mvvm.Ioc;
using Pamucuk.Mvvm.ViewModels;
using Pamucuk.UI.Controls.Windows;

namespace Molecula.Bootstrapping
{
    public class Bootstrapper
    {
        public ILocator Locator { get; private set; }

        private readonly IIocContainer _ioc;

        public Bootstrapper()
        {
            _ioc = new IocContainer();

            RegisterServices();
            RegisterViewModels();
            RegisterViews();
            RegisterWorkflowDesigner();

            Locator = new Locator(_ioc);
            _ioc.Register(() => Locator);
        }

        private void RegisterServices()
        {
            _ioc.Register<RelayCommandFactory, ICommandFactory>();
            _ioc.Register<WindowManager, IWindowManager>();
            _ioc.Register<ProgramManager, IProgramManager>();
        }

        private void RegisterViewModels()
        {
            _ioc.Register<LoginViewModel, ILoginViewModel>();
            _ioc.Register<MainMenuViewModel, IMainMenuViewModel>();
            _ioc.Register<CreateProgramViewModel>(
                () =>
                    programId =>
                    {
                        var type = (Application.Current.TryFindResource($"{programId}.Content.Template") as DataTemplate)?.DataType as Type;
                        if (type != null)
                        {
                            return _ioc.GetNewInstance(type) as IViewModelBase;
                        }
                        var vm = new ProgramViewModel(programId);
                        return vm;
                    });
        }

        private void RegisterViews()
        {
            _ioc.Register<Window>(() =>
            {
                var window = new AppWindow();
                LocatorExtension.SetLocator(window, Locator);
                return window;
            });
        }

        private void RegisterWorkflowDesigner()
        {
            _ioc.Register(() => new CreateWorkflowItem<IBaseNode>(type => (IBaseNode) _ioc.GetNewInstance(type)));
            _ioc.Register(() => new CreateWorkflowItem<INodeLink>(type => (INodeLink)_ioc.GetNewInstance(type)));
            _ioc.Register(() => new CreateWorkflowItem<INodeLinkPreview>(type => (INodeLinkPreview)_ioc.GetNewInstance(type)));
            _ioc.Register<WorkflowDesignerViewModel, IWorkflowDesignerViewModel>();
            _ioc.Register<Workspace, IWorkspace>();
            _ioc.Register<WorkflowItemToolbox, IWorkflowItemToolbox>();
            _ioc.Register<DesignerNode, IDesignerNode>();
            _ioc.Register<NodeLink, INodeLink>();
            _ioc.Register<NodeLinkPreview, INodeLinkPreview>();
            _ioc.Register<AssignNode, IAssignNode>();
            _ioc.Register<DebugNode, IDebugNode>();
            _ioc.Register<FlowNode, IFlowNode>();
            _ioc.Register<FunctionNode, IFunctionNode>();
            _ioc.Register<ReturnNode, IReturnNode>();
            _ioc.Register<SnippetNode, ISnippetNode>();
            _ioc.Register<StartNode, IStartNode>();
            _ioc.Register<SwitchNode, ISwitchNode>();
            _ioc.Register<ThrowNode, IThrowNode>();
        }
    }
}
