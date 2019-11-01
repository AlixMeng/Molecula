using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Nodes
{
    public class FunctionNode : BaseNode, IFunctionNode
    {
        public FunctionNode()
        {
            Text = "Function";
        }
    }
}