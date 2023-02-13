﻿using System.Globalization;

namespace Net.UI;

public class FalseToDoubleConverter : IValueConverter
{
    public static FalseToDoubleConverter Instance { get; } = new FalseToDoubleConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b && parameter is double d)
        {
            if (!b) return d;
        }

        return 1.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}