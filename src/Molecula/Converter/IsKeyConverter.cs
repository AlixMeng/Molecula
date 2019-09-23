using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Molecula.Converter
{
    public class IsKeyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var args = values.OfType<KeyEventArgs>().First();
            return Equals(args.Key, parameter);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}