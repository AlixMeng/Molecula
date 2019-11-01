using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Molecula.UI.Converter
{
    [ContentProperty(nameof(Mappings))]
    public class ValueMappingConverter : IValueConverter, IAddChild
    {
        private List<ValueMapping> _mappings;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IList Mappings => _mappings ??= new List<ValueMapping>();

        public object DefaultFrom { get; set; }
        public object DefaultTo { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => _mappings.FirstOrDefault(mapping => Equals(mapping.From, value)) ?? DefaultTo;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => _mappings.FirstOrDefault(mapping => Equals(mapping.To, value)) ?? DefaultFrom;

        public void AddChild(object value)
        {
            if (!(value is ValueMapping mapping))
                throw new InvalidCastException();

            Mappings.Add(mapping);
        }

        public void AddText(string text)
            => throw new NotImplementedException();
    }
}