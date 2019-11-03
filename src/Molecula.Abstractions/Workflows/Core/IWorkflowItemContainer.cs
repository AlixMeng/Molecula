using System.Windows.Input;

namespace Molecula.Abstractions.Workflows.Core
{
    public interface IWorkflowItemContainer
    {
        ICommand StartConnectionCommand { get; }
        ICommand MoveConnectionCommand { get; }
        ICommand StopConnectionCommand { get; }
        ICommand MoveNodeCommand { get; }
    }
}