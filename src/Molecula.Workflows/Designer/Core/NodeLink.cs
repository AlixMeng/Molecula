using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Core
{
    public class NodeLink : WorkflowItem, INodeLink
    {
        private IBaseNode _startNode;
        public IBaseNode StartNode
        {
            get => _startNode;
            set => Set(ref _startNode, value);
        }

        private IBaseNode _endNode;
        public IBaseNode EndNode
        {
            get => _endNode;
            set => Set(ref _endNode, value);
        }
    }
}
