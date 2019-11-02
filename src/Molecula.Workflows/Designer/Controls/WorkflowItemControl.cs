using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Controls
{
    public class WorkflowItemControl : Control
    {
        public static readonly RoutedEvent IsCheckedChangedEvent = EventManager.RegisterRoutedEvent(nameof(IsCheckedChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WorkflowItemControl));

        public event RoutedEventHandler IsCheckedChanged
        {
            add => AddHandler(IsCheckedChangedEvent, value);
            remove => RemoveHandler(IsCheckedChangedEvent, value);
        }

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

        static WorkflowItemControl()
        {
            EventManager.RegisterClassHandler(typeof(WorkflowItemControl), PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseLeftButtonDown));
        }

        protected WorkflowItemControl()
        {

        }

        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is WorkflowItemControl item)) return;
           
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                item.IsChecked = !item.IsChecked;
            }
            else if (!item.IsChecked)
            {
                item.IsChecked = true;
            }

            if(item.IsChecked)
            {
                item.Focus();
            }
        }
    }
}