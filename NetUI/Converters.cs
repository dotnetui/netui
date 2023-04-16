using Net.Essentials.Converters;
namespace Net.UI;

public class AndMultiConverter : AndPortableMultiConverter, IMultiValueConverter
{
    public static AndMultiConverter Instance { get; } = new AndMultiConverter();
}

public class UpperConverter : UpperPortableConverter, IValueConverter
{
    public static UpperConverter Instance { get; } = new UpperConverter();
}

public class LowerConverter : LowerPortableConverter, IValueConverter
{
    public static LowerConverter Instance { get; } = new LowerConverter();
}

public class TitleCaseConverter : TitleCasePortableConverter, IValueConverter
{
    public static TitleCaseConverter Instance { get; } = new TitleCaseConverter();
}

public class CountIsConverter : CountIsPortableConverter, IValueConverter
{
    public static CountIsConverter Instance { get; } = new CountIsConverter();
}

public class CountIsNotConverter : CountIsNotPortableConverter, IValueConverter
{
    public static CountIsNotConverter Instance { get; } = new CountIsNotConverter();
}

public class EqualsConverter : EqualsPortableConverter, IValueConverter
{
    public static EqualsConverter Instance { get; } = new EqualsConverter();
}

public class NotEqualsConverter : NotEqualsPortableConverter, IValueConverter
{
    public static NotEqualsConverter Instance { get; } = new NotEqualsConverter();
}

public class FalseToDoubleConverter : FalseToDoublePortableConverter, IPortableValueConverter
{
    public static FalseToDoubleConverter Instance { get; } = new FalseToDoubleConverter();
}

public class FalseToZeroConverter : FalseToZeroPortableConverter, IValueConverter
{
    public static FalseToZeroConverter Instance { get; private set; } = new FalseToZeroConverter();
}

public class FirstOrDefaultMultiConverter : FirstOrDefaultMultiPortableConverter, IMultiValueConverter
{
    public static FirstOrDefaultMultiConverter Instance { get; } = new FirstOrDefaultMultiConverter();
}

public class HasValueConverter : HasValuePortableConverter, IValueConverter
{
    public static HasValueConverter Instance { get; } = new HasValueConverter();
}

public class IsNullConverter : IsNullPortableConverter, IValueConverter
{
    public static IsNullConverter Instance { get; } = new IsNullConverter();
}

public class IsNotNullConverter : IsNotNullPortableConverter, IValueConverter
{
    public static IsNotNullConverter Instance { get; } = new IsNotNullConverter();
}

public class NotConverter : NotPortableConverter, IValueConverter
{
    public static NotConverter Instance { get; } = new NotConverter();
}

public class OrMultiConverter : OrMultiPortableConverter, IMultiValueConverter
{
    public static OrMultiConverter Instance { get; } = new OrMultiConverter();
}

public class WhiteSpaceConverter : WhiteSpacePortableConverter, IValueConverter
{
    public static WhiteSpaceConverter Instance { get; private set; } = new WhiteSpaceConverter();
}