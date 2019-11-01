using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Molecula.Workflows.Designer.Core;

namespace Molecula.Workflows.Designer.Nodes
{
    public class ReturnNode : BaseNode, IReturnNode
    {
        public ReturnNode()
        {
            OutputType = NodeOutputType.None;
            Text = "Return";
        }
    }
}