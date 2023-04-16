using System;
using System.Globalization;

namespace Net.Essentials.Converters
{
    public abstract class IsNullPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s) return string.IsNullOrWhiteSpace(s);
            return value is null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);
    }

    public abstract class IsNotNullPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s) return !string.IsNullOrWhiteSpace(s);
            return !(value is null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);
    }
}