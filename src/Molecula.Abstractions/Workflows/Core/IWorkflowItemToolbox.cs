using System.Collections.Generic;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IWorkflowItemToolbox
    {
        IEnumerable<IDesignerNode> Items { get; }
    }
}