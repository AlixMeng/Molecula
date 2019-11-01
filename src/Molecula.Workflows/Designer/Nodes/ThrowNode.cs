using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Molecula.Workflows.Designer.Core;

namespace Molecula.Workflows.Designer.Nodes
{
    public class ThrowNode : BaseNode, IThrowNode
    {
        public ThrowNode()
        {
            OutputType = NodeOutputType.None;
            Text = "Throw";
        }
    }
}