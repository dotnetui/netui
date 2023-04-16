using System;
using System.Globalization;

namespace Net.Essentials.Converters
{
    public abstract class UpperPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString()?.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);
    }

    public abstract class LowerPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString()?.ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);
    }

    public abstract class TitleCasePortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value?.ToString();
            if (s == null) return s;
            return culture.TextInfo.ToTitleCase(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);
    }
}