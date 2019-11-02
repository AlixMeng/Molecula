using System.Windows;
using System.Windows.Controls;

namespace Molecula.Workflows.Designer.Controls
{
    public class WorkflowItemToolboxContainerControl : ContentControl
    {
        static WorkflowItemToolboxContainerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkflowItemToolboxContainerControl), new FrameworkPropertyMetadata(typeof(WorkflowItemToolboxContainerControl)));
        }
    }
}