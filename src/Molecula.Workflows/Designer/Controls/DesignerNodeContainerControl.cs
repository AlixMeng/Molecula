using System.Windows;
using System.Windows.Markup;

namespace Molecula.Workflows.Designer.Controls
{
    [ContentProperty(nameof(Content))]
    public class DesignerNodeContainerControl : WorkflowItemContainerControl
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(DesignerNodeContainerControl));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        static DesignerNodeContainerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerNodeContainerControl), new FrameworkPropertyMetadata(typeof(DesignerNodeContainerControl)));
        }

    }
}