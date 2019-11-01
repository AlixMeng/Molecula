using System.Windows;

namespace Molecula.Workflows.Designer.Controls
{
    public class NodeInputControl : NodeControlThumbBase
    {
        static NodeInputControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeInputControl), new FrameworkPropertyMetadata(typeof(NodeInputControl)));
        }
    }
}
