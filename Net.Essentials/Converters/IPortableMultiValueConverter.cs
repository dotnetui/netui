using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Net.Essentials.Converters
{
    public interface IPortableMultiValueConverter
    {
        object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture);
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
