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
    public class DragCompletedArgsConverter : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new DragCompletedArgsConverter();

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