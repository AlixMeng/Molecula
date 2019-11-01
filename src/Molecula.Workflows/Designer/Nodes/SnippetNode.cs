using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Workflows.Designer.Nodes
{
    public class SnippetNode : BaseNode, ISnippetNode
    {
        public SnippetNode()
        {
            Text = "Snippet";
        }
    }
}