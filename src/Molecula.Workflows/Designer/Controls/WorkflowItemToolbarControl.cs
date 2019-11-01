using System.Windows;

namespace Molecula.Workflows.Designer.Controls
{
    public class WorkflowItemToolbarControl : WorkflowItemContainer
    {
        static WorkflowItemToolbarControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkflowItemToolbarControl), new FrameworkPropertyMetadata(typeof(WorkflowItemToolbarControl)));
        }
    }
}
