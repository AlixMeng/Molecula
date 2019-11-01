using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Nodes
{
    public class StartNode : BaseNode, IStartNode
    {
        public StartNode()
        {
            HasInput = false;
            IsRemovable = false;
            Text = "Start";
        }
    }
}