using System.Collections.Generic;
using System.Windows.Input;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IWorkflowItemToolbox
    {
        IEnumerable<IDesignerNode> Items { get; }
        ICommand StartConnectionCommand { get; }
        ICommand MoveConnectionCommand { get; }
        ICommand StopConnectionCommand { get; }
        ICommand MoveNodeCommand { get; }
    }
}