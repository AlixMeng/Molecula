using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Nodes
{
    public class FlowNode : BaseNode, IFlowNode
    {
        public FlowNode()
        {
            Text = "Flow";
        }
    }
}