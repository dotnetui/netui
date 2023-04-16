using System;
using System.Collections;
using System.Globalization;

namespace Net.Essentials.Converters
{
    public abstract class CountIsPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? p = null;
            if (int.TryParse(parameter?.ToString(), out var i))
                p = i;
            if (p == null) return value == null;
            if (value is IList e)
                return e?.Count == p;
            if (value is ICollection c)
                return c?.Count == p;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            null;
    }

    public abstract class CountIsNotPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? p = null;
            if (int.TryParse(parameter?.ToString(), out var i))
                p = i;
            if (p == null) return value != null;
            if (value is IList e)
                return e?.Count != p;
            if (value is ICollection c)
                return c?.Count != p;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            null;
    }
}