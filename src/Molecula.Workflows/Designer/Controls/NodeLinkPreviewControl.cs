using System.Windows;
using System.Windows.Controls;

namespace Molecula.Workflows.Designer.Controls
{
    public class NodeLinkPreviewControl : Control
    {
        static NodeLinkPreviewControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeLinkPreviewControl), new FrameworkPropertyMetadata(typeof(NodeLinkPreviewControl)));
        }
    }
}
