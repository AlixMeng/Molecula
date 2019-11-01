using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace Molecula.Workflows.Designer.Controls
{
    [ContentProperty(nameof(Content))]
    public class NodeControlThumbBase : Thumb
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(NodeControlThumbBase), new PropertyMetadata(null));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
    }
}