using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Pamucuk.Mvvm.Observables;

namespace Molecula.Workflows.Designer.Core
{
    public class WorkflowItemToolbox : ObservableObject, IWorkflowItemToolbox
    {
        private readonly Func<IDesignerNode> _designerNodeFactory;
        private ObservableCollection<IDesignerNode> _items;
        public IEnumerable<IDesignerNode> Items => _items;

        public ICommand StartConnectionCommand { get; } = null;
        public ICommand MoveConnectionCommand { get; } = null;
        public ICommand StopConnectionCommand { get; } = null;
        public ICommand MoveNodeCommand { get; } = null;

        public WorkflowItemToolbox(Func<IDesignerNode> designerNodeFactory)
        {
            _designerNodeFactory = designerNodeFactory;
            LoadDesignerNodes();
        }

        private void LoadDesignerNodes()
        {
            var nodes =
                AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .AsParallel()
                    .SelectMany(asm => asm.ExportedTypes)
                    .Where(typeof(IBaseNode).IsAssignableFrom)
                    .Where(type => !typeof(IStartNode).IsAssignableFrom(type))
                    .Where(type => type.IsClass)
                    .Where(type => !type.IsAbstract)
                    .Where(type => !type.IsGenericType)
                    .Where(type => !type.IsGenericTypeDefinition)
                    .Where(type => type.GetConstructors().Any(ctor => !ctor.GetParameters().Any()))
                    .OrderBy(type => $"{type.Namespace}.{type.Name}")
                    .Select(CreateDesignerNode);
            var items = new ObservableCollection<IDesignerNode>(nodes);
            Set(ref _items, items, nameof(Items));
        }

        private IDesignerNode CreateDesignerNode(Type nodeType)
        {
            var node = _designerNodeFactory();
            node.NodeType = nodeType;
            return node;
        }
    }
}
