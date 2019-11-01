using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Controls
{
    [ContentProperty(nameof(Content))]
    public class NodeControl : WorkflowItemControl
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(NodeControl), new PropertyMetadata(null));

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(NodeControl), new PropertyMetadata(null));
        
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        
        static NodeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeControl), new FrameworkPropertyMetadata(typeof(NodeControl)));
        }

        public NodeControl()
        {
            Loaded += (sender, args) =>
            {
                var contentPresenter = this.FindParent<ContentPresenter>(false);
                BindingOperations.SetBinding(contentPresenter, Canvas.LeftProperty, new Binding()
                {
                    Path = new PropertyPath("(0)", Canvas.LeftProperty),
                    Mode = BindingMode.TwoWay,
                    Source = this,
                });
                BindingOperations.SetBinding(contentPresenter, Canvas.TopProperty, new Binding()
                {
                    Path = new PropertyPath("(0)", Canvas.TopProperty),
                    Mode = BindingMode.TwoWay,
                    Source = this,
                });
            };
        }
    }
}
