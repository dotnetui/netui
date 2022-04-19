using System.Globalization;

namespace Net.UI;

public class FalseToZeroConverter : IValueConverter
{
    public static FalseToZeroConverter Instance { get; private set; } = new FalseToZeroConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && !b ? 0.0 : (object)1.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}