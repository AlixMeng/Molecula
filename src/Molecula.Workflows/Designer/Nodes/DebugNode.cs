using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Nodes
{
    public class DebugNode : BaseNode, IDebugNode
    {
        public DebugNode()
        {
            Text = "Debug";
        }
    }
}