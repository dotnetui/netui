using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Net.Essentials.Converters
{
    public interface IPortableValueConverter
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
