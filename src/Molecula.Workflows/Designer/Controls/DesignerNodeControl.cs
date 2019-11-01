using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Molecula.Workflows.Designer.Nodes;

namespace Molecula.Workflows.Designer.Controls
{
    public class DesignerNodeControl : Button
    {
        public static readonly DependencyProperty NodeTypeProperty =
            DependencyProperty.Register(nameof(NodeType), typeof(Type), typeof(DesignerNodeControl), new PropertyMetadata(default, OnNodeTypeChanged));

        public Type NodeType
        {
            get => (Type)GetValue(NodeTypeProperty);
            set => SetValue(NodeTypeProperty, value);
        }

        private static void OnNodeTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DesignerNodeControl designer)) return;
            if (!(e.NewValue is Type type)) return;
            var node = (BaseNode)Activator.CreateInstance(designer.NodeType);
            designer.Content = node;
        }

        static DesignerNodeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerNodeControl), new FrameworkPropertyMetadata(typeof(DesignerNodeControl)));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            e.Handled = true;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            e.Handled = true;
            OnClick();
        }
    }
}
