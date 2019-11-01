using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Controls
{
    public class WorkspaceControl : WorkflowItemContainer
    {
        public static readonly DependencyProperty NodeLinkStartProperty =
            DependencyProperty.Register(nameof(NodeLinkStart), typeof(Point?), typeof(WorkspaceControl), new PropertyMetadata(default));

        public Point? NodeLinkStart
        {
            get => (Point?)GetValue(NodeLinkStartProperty);
            set => SetValue(NodeLinkStartProperty, value);
        }

        public static readonly DependencyProperty NodeLinkEndProperty =
            DependencyProperty.Register(nameof(NodeLinkEnd), typeof(Point?), typeof(WorkspaceControl), new PropertyMetadata(default));

        public Point? NodeLinkEnd
        {
            get => (Point?)GetValue(NodeLinkEndProperty);
            set => SetValue(NodeLinkEndProperty, value);
        }

        private static readonly DependencyProperty HorizontalScrollOffsetProperty =
            DependencyProperty.Register(nameof(HorizontalScrollOffset), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnHorizontalScrollOffsetChanged));

        private double HorizontalScrollOffset
        {
            get => (double)GetValue(HorizontalScrollOffsetProperty);
            set => SetValue(HorizontalScrollOffsetProperty, value);
        }

        private static void OnHorizontalScrollOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(LeftOffsetProperty, e.NewValue);

        private static readonly DependencyProperty VerticalScrollOffsetProperty =
            DependencyProperty.Register(nameof(VerticalScrollOffset), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnVerticalScrollOffsetChanged));

        private double VerticalScrollOffset
        {
            get => (double)GetValue(VerticalScrollOffsetProperty);
            set => SetValue(VerticalScrollOffsetProperty, value);
        }

        private static void OnVerticalScrollOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(TopOffsetProperty, e.NewValue);

        public static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register(nameof(LeftOffset), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnLeftOffsetChanged));

        public double LeftOffset
        {
            get => (double)GetValue(LeftOffsetProperty);
            set => SetValue(LeftOffsetProperty, value);
        }

        private static void OnLeftOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is double left)) return;
            var viewer = d.FindChild<ScrollViewer>("PART_ScrollViewer", false);
            if (Math.Abs(viewer.HorizontalOffset - left) > 0.0001)
                viewer.ScrollToHorizontalOffset(left);
        }

        public static readonly DependencyProperty TopOffsetProperty =
            DependencyProperty.Register(nameof(TopOffset), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnTopOffsetChanged));

        public double TopOffset
        {
            get => (double)GetValue(TopOffsetProperty);
            set => SetValue(TopOffsetProperty, value);
        }

        private static void OnTopOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is double top)) return;
            var viewer = d.FindChild<ScrollViewer>("PART_ScrollViewer", false);
            if (Math.Abs(viewer.VerticalOffset - top) > 0.0001)
                viewer.ScrollToVerticalOffset(top);
        }

        static WorkspaceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkspaceControl), new FrameworkPropertyMetadata(typeof(WorkspaceControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            BindScrollOffsets();
        }

        private void BindScrollOffsets()
        {
            var viewer = this.FindChild<ScrollViewer>("PART_ScrollViewer", false);

            var horizontalOffsetBinding = CreateScrollOffsetBinding(viewer, ScrollViewer.HorizontalOffsetProperty);
            BindingOperations.SetBinding(this, HorizontalScrollOffsetProperty, horizontalOffsetBinding);

            var verticalOffsetBinding = CreateScrollOffsetBinding(viewer, ScrollViewer.VerticalOffsetProperty);
            BindingOperations.SetBinding(this, VerticalScrollOffsetProperty, verticalOffsetBinding);
        }

        private static Binding CreateScrollOffsetBinding(ScrollViewer viewer, DependencyProperty property)
        {
            var offsetBinding = new Binding
            {
                Path = new PropertyPath("(0)", property),
                Source = viewer,
            };
            return offsetBinding;
        }
    }
}
