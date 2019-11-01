using System.Windows;

namespace Molecula.Workflows.Designer.Controls
{
    public class NodeMoveControl : NodeControlThumbBase
    {
        static NodeMoveControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeMoveControl), new FrameworkPropertyMetadata(typeof(NodeMoveControl)));
        }
    }
}