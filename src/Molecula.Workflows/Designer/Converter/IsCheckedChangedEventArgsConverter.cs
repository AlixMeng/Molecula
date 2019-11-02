using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Molecula.Workflows.Designer.Controls;

namespace Molecula.Workflows.Designer.Converter
{
    public class IsCheckedChangedEventArgsConverter : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new IsCheckedChangedEventArgsConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values.OfType<RoutedEventArgs>().First();
            return (args.OriginalSource as WorkflowItemControl)?.DataContext;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}