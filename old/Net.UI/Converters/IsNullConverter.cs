﻿using System.Globalization;

namespace Net.UI;

public class IsNullConverter : IValueConverter
{
    public static IsNullConverter Instance { get; } = new IsNullConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s) return string.IsNullOrWhiteSpace(s);
        return value is null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Convert(value, targetType, parameter, culture);
}

public class IsNotNullConverter : IValueConverter
{
    public static IsNotNullConverter Instance { get; } = new IsNotNullConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s) return !string.IsNullOrWhiteSpace(s);
        return !(value is null);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Convert(value, targetType, parameter, culture);
}