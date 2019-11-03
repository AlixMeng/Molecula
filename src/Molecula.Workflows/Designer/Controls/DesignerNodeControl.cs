using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Controls
{
    [ContentProperty(nameof(Content))]
    public class DesignerNodeControl : Thumb
    {
        static DesignerNodeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerNodeControl), new FrameworkPropertyMetadata(typeof(DesignerNodeControl)));
            EventManager.RegisterClassHandler(typeof(DesignerNodeControl), DragStartedEvent, new DragStartedEventHandler(OnDragStarted));
            EventManager.RegisterClassHandler(typeof(DesignerNodeControl), DragDeltaEvent, new DragDeltaEventHandler(OnDragDelta));
        }

        private Window _nodePreview;

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(DesignerNodeControl));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        
        public static readonly DependencyProperty DragStartPointProperty =
            DependencyProperty.Register(nameof(DragStartPoint), typeof(Point), typeof(DesignerNodeControl));

        public Point DragStartPoint
        {
            get => (Point) GetValue(DragStartPointProperty);
            set => SetValue(DragStartPointProperty, value);
        }
        
        public static readonly DependencyProperty DragStartScreenPointProperty =
            DependencyProperty.Register(nameof(DragStartScreenPoint), typeof(Point), typeof(DesignerNodeControl));

        public Point DragStartScreenPoint
        {
            get => (Point) GetValue(DragStartScreenPointProperty);
            set => SetValue(DragStartScreenPointProperty, value);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            DragStartPoint = e.GetPosition(this);
            DragStartScreenPoint = PointToScreen(new Point(0, 0));
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            _nodePreview?.Close();
            _nodePreview = null;
            DragStartPoint = default;
            DragStartScreenPoint = default;
        }

        private static void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            if (!(sender is DesignerNodeControl node)) return;
            var point = node.PointToScreen(new Point(0, 0));
            var window = new Window
            {
                Style = node.TryFindResource("DesignerNodeControlWindowStyle") as Style,
                Owner = node.FindParent<Window>(false),
                Left = point.X,
                Top = point.Y,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Content = node.Content
            };
            window.Show();
            node._nodePreview = window;
        }

        private static void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is DesignerNodeControl node)) return;
            var window = node._nodePreview;
            var point = node.DragStartScreenPoint;
            window.Left = point.X + e.HorizontalChange;
            window.Top = point.Y + e.VerticalChange;
        }

    }
}
