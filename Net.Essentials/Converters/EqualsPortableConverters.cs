using System;
using System.Globalization;

namespace Net.Essentials.Converters
{
    public abstract class EqualsPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            null;
    }

    public abstract class NotEqualsPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() != parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            null;
    }
}