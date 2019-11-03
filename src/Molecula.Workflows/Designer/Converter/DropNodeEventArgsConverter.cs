using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Molecula.Abstractions.Workflows.Core;
using Molecula.Workflows.Designer.Controls;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Converter
{
    public class DropNodeEventArgsConverter : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new DropNodeEventArgsConverter();

        private static readonly (double X, double Y, IDesignerNode Node)
            DefaultConvertResult = (0.0, 0.0, null);

        private DropNodeEventArgsConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values.OfType<DragCompletedEventArgs>().First();

            var nodeControl = FindNodeControl(args);
            if (nodeControl == null) return DefaultConvertResult;
            
            var workspace = FindWorkspace(nodeControl, args);
            if (workspace == null) return DefaultConvertResult;

            var container = workspace.FindChild<FrameworkElement>("PART_ItemsContainer", false);

            var point = CalculateTargetPoint(container, nodeControl, args);

            var designerNode = nodeControl.DataContext as IDesignerNode;
            return (point.X, point.Y, designerNode);
        }

        private static DesignerNodeControl FindNodeControl(DragCompletedEventArgs args)
        {
            var nodeControl = (args.OriginalSource as DesignerNodeControl)
                              ?? (args.OriginalSource as FrameworkElement)?.FindParent<DesignerNodeControl>(false);
            return nodeControl;
        }

        private Point CalculateTargetPoint(FrameworkElement workspace, DesignerNodeControl nodeControl, DragCompletedEventArgs args)
        {
            var point = nodeControl.TranslatePoint(new Point(0, 0), workspace);
            point.X += args.HorizontalChange;
            point.Y += args.VerticalChange;
            return point;
        }

        private WorkspaceControl FindWorkspace(DesignerNodeControl nodeControl, DragCompletedEventArgs args)
        {
            var window = nodeControl.FindParent<Window>(false);
            if (window == null) return null;

            var currentPoint = new Point(
                nodeControl.DragStartPoint.X + args.HorizontalChange,
                nodeControl.DragStartPoint.Y + args.VerticalChange);

            var point = nodeControl.TranslatePoint(currentPoint, window);

            var children = new List<WorkspaceControl>();

            HitTestResultBehavior CheckHitTestResult(HitTestResult result)
            {
                var behavior = HitTestResultBehavior.Continue;
                if ((result.VisualHit is WorkspaceControl hit || (hit = result.VisualHit.FindParent<WorkspaceControl>(false)) != null)
                    && hit.Visibility == Visibility.Visible)
                {
                    children.Add(hit);
                    behavior = HitTestResultBehavior.Stop;
                }
                return behavior;
            }

            VisualTreeHelper.HitTest(window, null, CheckHitTestResult, new PointHitTestParameters(point));
            return children.FirstOrDefault();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}