using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace Molecula.Workflows.Designer.Converter
{
    public class CoordinatesToPointConverter : IMultiValueConverter
    {
        public static readonly CoordinatesToPointConverter Instance = new CoordinatesToPointConverter();

        private CoordinatesToPointConverter()
        {

        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (values[0] is double x && values[1] is double y) 
                ? new Point(x, y) 
                : default;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return value is Point point
                ? new object[] {point.X, point.Y}
                : new object[2];
        }
    }
}
