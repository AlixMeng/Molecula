using System.Windows;
using System.Windows.Controls.Primitives;

namespace Molecula.Workflows.Designer.Controls
{
    public class NodeOutputControl : NodeControlThumbBase
    {
        static NodeOutputControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeOutputControl), new FrameworkPropertyMetadata(typeof(NodeOutputControl)));
        }
    }
}