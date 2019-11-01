using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Controls
{
    public class WorkflowItemControl : Control
    {
        public static readonly RoutedEvent IsCheckedChangedEvent = EventManager.RegisterRoutedEvent("IsCheckedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NodeLinkControl));

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(WorkflowItemControl),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        private static void OnIsCheckedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(obj is WorkflowItemControl item)) return;
            item.RaiseEvent(new RoutedEventArgs(IsCheckedChangedEvent, item));
        }

        private Point _buttonDownPoint;

        static WorkflowItemControl()
        {
            EventManager.RegisterClassHandler(typeof(WorkflowItemControl), PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseLeftButtonDown));
            EventManager.RegisterClassHandler(typeof(WorkflowItemControl), PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(OnPreviewMouseLeftButtonUp));
        }

        protected WorkflowItemControl()
        {

        }

        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is WorkflowItemControl item)) return;
            item._buttonDownPoint = e.GetPosition(item.FindParent<Window>());
        }

        private static void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is WorkflowItemControl item)) return;
            if(e.GetPosition(item.FindParent<Window>()) == item._buttonDownPoint)
                item.IsChecked = !item.IsChecked;
        }
    }
}