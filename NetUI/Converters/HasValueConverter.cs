﻿using System.Collections;

using Net.Essentials;
namespace Net.UI;

public class HasValueConverter : IValueConverter
{
    public static HasValueConverter Instance { get; } = new HasValueConverter();

    public static bool HasValue(object value)
    {
        if (value is string text)
            return text?.HasValue() ?? false;
        if (value is IList e)
            return e?.Count > 0;
        if (value is ICollection c)
            return c?.Count > 0;
        return value != null;
    }

    public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
        return HasValue(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
        return value;
    }
}