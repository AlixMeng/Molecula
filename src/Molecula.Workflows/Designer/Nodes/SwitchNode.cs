using Molecula.Abstractions.Workflows.Core;
using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Nodes
{
    public class SwitchNode : BaseNode, ISwitchNode
    {
        public SwitchNode()
        {
            OutputType = NodeOutputType.Multiple;
            Text = "Switch";
            LinkOutputType = typeof(IConditionalLink);
        }
    }
}