using System;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Pamucuk.Mvvm.Observables;

namespace Molecula.Workflows.Designer.Nodes
{
    public class DesignerNode : ObservableObject, IDesignerNode
    {
        private readonly CreateWorkflowItem<IBaseNode> _nodeFactory;

        private Type _nodeType;
        public Type NodeType
        {
            get => _nodeType;
            set => SetNodeType(ref _nodeType, value);
        }

        public string Namespace => _nodeType.Namespace;

        private IBaseNode _node;
        public IBaseNode Node
        {
            get => _node;
            protected set => Set(ref _node, value);
        }

        public DesignerNode(Func<CreateWorkflowItem<IBaseNode>> getNodeFactory)
        {
            _nodeFactory = getNodeFactory();
        }

        private void SetNodeType(ref Type field, Type value)
        {
            if (!Set(ref _nodeType, value, nameof(NodeType))) return;
            Node = _nodeFactory(_nodeType);
            OnPropertyChanged(nameof(Namespace));
        }
    }
}
