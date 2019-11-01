using Molecula.Abstractions.Workflows.Nodes;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface INodeLink : IWorkflowItem
    {
        IBaseNode StartNode { get; set; }
        IBaseNode EndNode { get; set; }
    }
}