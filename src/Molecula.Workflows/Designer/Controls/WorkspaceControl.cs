using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Controls
{
    public class WorkspaceControl : WorkflowItemContainerControl
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

        private static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register("LeftOffset", typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnLeftOffsetChanged));

        private static void OnLeftOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(HorizontalScrollOffsetProperty, e.NewValue);

        private static readonly DependencyProperty TopOffsetProperty =
            DependencyProperty.Register("TopOffset", typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnTopOffsetChanged));

        private static void OnTopOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(VerticalScrollOffsetProperty, e.NewValue);

        private static readonly DependencyProperty ViewWidthProperty =
            DependencyProperty.Register("ViewWidth", typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnViewWidthChanged));

        private static void OnViewWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(ViewportWidthProperty, e.NewValue);

        private static readonly DependencyProperty ViewHeightProperty =
            DependencyProperty.Register("ViewHeight", typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnViewHeightChanged));

        private static void OnViewHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(ViewportHeightProperty, e.NewValue);

        public static readonly DependencyProperty HorizontalScrollOffsetProperty =
            DependencyProperty.Register(nameof(HorizontalScrollOffset), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnHorizontalScrollOffsetChanged));

        public double HorizontalScrollOffset
        {
            get => (double)GetValue(HorizontalScrollOffsetProperty);
            set => SetValue(HorizontalScrollOffsetProperty, value);
        }

        private static void OnHorizontalScrollOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is double left)) return;
            var viewer = d.FindChild<ScrollViewer>("PART_ScrollViewer", false);
            if (Math.Abs(viewer.HorizontalOffset - left) > 0.0001)
                viewer.ScrollToHorizontalOffset(left);
        }

        public static readonly DependencyProperty VerticalScrollOffsetProperty =
            DependencyProperty.Register(nameof(VerticalScrollOffset), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double), OnVerticalScrollOffsetChanged));

        public double VerticalScrollOffset
        {
            get => (double)GetValue(VerticalScrollOffsetProperty);
            set => SetValue(VerticalScrollOffsetProperty, value);
        }

        public static readonly DependencyProperty ViewportWidthProperty =
            DependencyProperty.Register(nameof(ViewportWidth), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double)));

        public double ViewportWidth
        {
            get => (double)GetValue(ViewportWidthProperty);
            set => SetValue(ViewportWidthProperty, value);
        }

        public static readonly DependencyProperty ViewportHeightProperty =
            DependencyProperty.Register(nameof(ViewportHeight), typeof(double), typeof(WorkspaceControl), new PropertyMetadata(default(double)));

        public double ViewportHeight
        {
            get => (double)GetValue(ViewportHeightProperty);
            set => SetValue(ViewportHeightProperty, value);
        }

        private static void OnVerticalScrollOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
            BindScrollViewer();
        }

        private void BindScrollViewer()
        {
            var viewer = this.FindChild<ScrollViewer>("PART_ScrollViewer", false);

            var horizontalOffsetBinding = CreateScrollViewerBinding(viewer, ScrollViewer.HorizontalOffsetProperty, BindingMode.OneWay);
            BindingOperations.SetBinding(this, LeftOffsetProperty, horizontalOffsetBinding);

            var verticalOffsetBinding = CreateScrollViewerBinding(viewer, ScrollViewer.VerticalOffsetProperty, BindingMode.OneWay);
            BindingOperations.SetBinding(this, TopOffsetProperty, verticalOffsetBinding);

            var viewportWidthBinding = CreateScrollViewerBinding(viewer, ScrollViewer.ViewportWidthProperty, BindingMode.OneWay);
            BindingOperations.SetBinding(this, ViewWidthProperty, viewportWidthBinding);

            var viewportHeightBinding = CreateScrollViewerBinding(viewer, ScrollViewer.ViewportHeightProperty, BindingMode.OneWay);
            BindingOperations.SetBinding(this, ViewHeightProperty, viewportHeightBinding);
        }

        private static Binding CreateScrollViewerBinding(ScrollViewer viewer, DependencyProperty property, BindingMode mode)
        {
            var binding = new Binding
            {
                Path = new PropertyPath("(0)", property),
                Source = viewer,
                Mode = mode,
            };
            return binding;
        }
    }
}
