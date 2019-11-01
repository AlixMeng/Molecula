using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Molecula.Workflows.Designer.Controls;

namespace Molecula.Workflows.Designer.Converter
{
    public class NodeLinkStrokeConverter : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new NodeLinkStrokeConverter();

        private static readonly Random Random = new Random(10);

        public Brush DefaultBrush { get; set; }

        public NodeLinkStrokeConverter()
        {
            var defaultBrush = new LinearGradientBrush(Colors.Black, Colors.CornflowerBlue, new Point(0, 0), new Point(1, 0));
            defaultBrush.Freeze();
            DefaultBrush = defaultBrush;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double startX)
                || !(values[1] is double startWidth) 
                || !(values[2] is double endX) 
                || !(values[3] is string itemType) 
                || !(values[4] is NodeLinkControl nodeLinkControl))
                return DefaultBrush;

            if(!(nodeLinkControl.TryFindResource($"{itemType}.Background") is SolidColorBrush endBrush))
                return DefaultBrush;

            startX += startWidth;
            var startPoint = new Point(startX < endX ? 0 : 1, 0);
            var endPoint = new Point(startX < endX ? 1 : 0, 0);
            return new LinearGradientBrush(Color.FromRgb(60, 60, 60), endBrush.Color, startPoint, endPoint);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}