using System.Globalization;

namespace Net.UI;

public class WhiteSpaceConverter : IValueConverter
{
    public static WhiteSpaceConverter Instance { get; private set; } = new WhiteSpaceConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value ?? " ";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
