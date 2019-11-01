using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Nodes
{
    public class AssignNode : BaseNode, IAssignNode
    {
        public AssignNode()
        {
            Text = "Assign";
        }
    }
}