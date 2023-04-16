using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Net.Essentials.Converters
{
    public abstract class FirstOrDefaultMultiPortableConverter : IPortableMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((IEnumerable<object>)values, targetType, parameter, culture);
        }

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((IEnumerable<object>)values, targetType, parameter, culture);
        }

        object Convert(IEnumerable<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (HasValuePortableConverter.HasValue(value))
                        return value;
                }
            }

            return parameter;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (!(value is bool b) || targetTypes.Any(t => !t.IsAssignableFrom(typeof(bool))))
            {
                return null;
            }

            if (b)
            {
                return targetTypes.Select(t => (object)true).ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}