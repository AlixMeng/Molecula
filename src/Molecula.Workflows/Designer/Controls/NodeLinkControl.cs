using System.Windows;

namespace Molecula.Workflows.Designer.Controls
{
    public class NodeLinkControl : WorkflowItemControl
    {
        static NodeLinkControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeLinkControl), new FrameworkPropertyMetadata(typeof(NodeLinkControl)));
        }
    }
}