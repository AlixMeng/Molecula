using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Molecula.Workflows.Designer.Controls;
using Pamucuk.UI.Extensions;

namespace Molecula.Workflows.Designer.Converter
{
    public class DragDeltaArgsConverter : IMultiValueConverter
    {
        public static readonly DragDeltaArgsConverter Instance = new DragDeltaArgsConverter();

        private static readonly (double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object DataContext)
            DefaultConvertResult = (0.0, 0.0, 0.0, 0.0, 0.0, 0.0, null);

        private DragDeltaArgsConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values?.OfType<DragDeltaEventArgs>().FirstOrDefault();
            var node = (args?.OriginalSource as FrameworkElement)?.FindParent<NodeControl>(false);
            return node == null
                ? DefaultConvertResult
                : (args.HorizontalChange, args.VerticalChange, X: Canvas.GetLeft(node), Y: Canvas.GetTop(node), node.Width, node.Height, node.DataContext);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NodeDragStartedArgsConverter : IMultiValueConverter
    {
        public static readonly NodeDragStartedArgsConverter Instance = new NodeDragStartedArgsConverter();

        private static readonly (double HorizontalOffset, double VerticalOffset, double X, double Y, double Width, double Height, object DataContext) 
            DefaultConvertResult = (0.0, 0.0, 0.0, 0.0, 0.0, 0.0, null);

        private NodeDragStartedArgsConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values?.OfType<DragStartedEventArgs>().FirstOrDefault();
            var node = (args?.OriginalSource as FrameworkElement)?.FindParent<NodeControl>(false);
            return node == null
                ? DefaultConvertResult
                : (args.HorizontalOffset, args.VerticalOffset, X: Canvas.GetLeft(node), Y: Canvas.GetTop(node), node.Width, node.Height, node.DataContext);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DragCompletedArgsConverter : IMultiValueConverter
    {
        public static readonly DragCompletedArgsConverter Instance = new DragCompletedArgsConverter();

        private static readonly (double HorizontalChange, double VerticalChange, double X, double Y, double Width, double Height, object DataContext)
            DefaultConvertResult = (0.0, 0.0, 0.0, 0.0, 0.0, 0.0, null);

        private DragCompletedArgsConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values?.OfType<DragCompletedEventArgs>().FirstOrDefault();
            var node = (args?.OriginalSource as FrameworkElement)?.FindParent<NodeControl>(false);
            return node == null
                ? DefaultConvertResult
                : (args.HorizontalChange, args.VerticalChange, X: Canvas.GetLeft(node), Y: Canvas.GetTop(node), node.Width, node.Height, node.DataContext);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
