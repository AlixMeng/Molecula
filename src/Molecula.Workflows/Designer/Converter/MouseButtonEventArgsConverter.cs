using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Molecula.Workflows.Designer.Converter
{
    public class MouseButtonEventArgsConverter : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new MouseButtonEventArgsConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values.OfType<MouseButtonEventArgs>().First();
            return (args.OriginalSource as FrameworkElement)?.DataContext;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}