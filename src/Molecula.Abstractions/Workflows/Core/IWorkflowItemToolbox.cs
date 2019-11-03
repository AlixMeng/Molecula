using System.Collections.Generic;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IWorkflowItemToolbox : IWorkflowItemContainer
    {
        IEnumerable<IDesignerNode> Items { get; }
    }
}