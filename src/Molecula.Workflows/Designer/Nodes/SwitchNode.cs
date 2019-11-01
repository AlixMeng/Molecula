using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;
using Molecula.Workflows.Designer.Core;

namespace Molecula.Workflows.Designer.Nodes
{
    public class SwitchNode : BaseNode, ISwitchNode
    {
        public SwitchNode()
        {
            OutputType = NodeOutputType.Multiple;
            Text = "Switch";
        }
    }
}