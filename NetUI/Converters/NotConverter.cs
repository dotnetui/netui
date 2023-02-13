﻿using System.Globalization;

namespace Net.UI;

public class NotConverter : IValueConverter
{
    public static NotConverter Instance { get; } = new NotConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b)
            return !b;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Convert(value, targetType, parameter, culture);
}