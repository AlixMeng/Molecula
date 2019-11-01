using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Molecula.Workflows.Designer.Converter
{
    public class KeyEventArgsConverter : IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new KeyEventArgsConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values.OfType<KeyEventArgs>().First();
            return (args.Key, args.KeyboardDevice.Modifiers);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
