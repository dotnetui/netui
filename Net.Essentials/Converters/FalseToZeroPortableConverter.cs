using System;
using System.Globalization;

namespace Net.Essentials.Converters
{
    public abstract class FalseToZeroPortableConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && !b ? 0.0 : (object)1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}