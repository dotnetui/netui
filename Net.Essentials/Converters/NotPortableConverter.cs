using System;
using System.Globalization;

namespace Net.Essentials.Converters
{
    public abstract class NotPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);
    }
}