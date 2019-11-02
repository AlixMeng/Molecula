using System.Windows;

namespace Molecula.Workflows.Designer.Controls
{
    public class WorkflowItemToolboxControl : WorkflowItemContainer
    {
        static WorkflowItemToolboxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkflowItemToolboxControl), new FrameworkPropertyMetadata(typeof(WorkflowItemToolboxControl)));
        }
    }
}
