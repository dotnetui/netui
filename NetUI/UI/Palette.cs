namespace Net.UI;

public static class Palette
{
    public static Color Default { get; } = new Color();
    public static Color Black { get; } = Color.FromArgb("#000");
    public static Color White { get; } = Color.FromArgb("#FFF");
    public static Color Transparent { get; } = Color.FromArgb("#00000000");
    public static Color LightGray { get; } = Color.FromArgb("#D3D3D3");
    public static Color Silver { get; } = Color.FromArgb("#C0C0C0");
    public static Color DarkGray { get; } = Color.FromArgb("#A9A9A9");
    public static Color Gray { get; } = Color.FromArgb("#808080");
    public static Color DimGray { get; } = Color.FromArgb("#696969");

    public static Color Color0 => Colors[0];
    public static Color Color1 => Colors[1];
    public static Color Color2 => Colors[2];
    public static Color Color3 => Colors[3];
    public static Color Color4 => Colors[4];
    public static Color Color5 => Colors[5];
    public static Color Color6 => Colors[6];
    public static Color Color7 => Colors[7];
    public static Color Color8 => Colors[8];
    public static Color Color9 => Colors[9];
    public static Color Color10 => Colors[10];
    public static Color Color11 => Colors[11];
    public static Color Color12 => Colors[12];
    public static Color Color13 => Colors[13];
    public static Color Color14 => Colors[14];
    public static Color Color15 => Colors[15];
    public static Color Color16 => Colors[16];
    public static Color Color17 => Colors[17];
    public static Color Color18 => Colors[18];
    public static Color Color19 => Colors[19];

    public static readonly Color[] Colors = new[]
    {
            Color.FromArgb("#c62828"),
            Color.FromArgb("#6A1B9A"),
            Color.FromArgb("#283593"),
            Color.FromArgb("#0277BD"),
            Color.FromArgb("#00695C"),
            Color.FromArgb("#558B2F"),
            Color.FromArgb("#F9A825"),
            Color.FromArgb("#EF6C00"),
            Color.FromArgb("#4E342E"),
            Color.FromArgb("#37474F"),
            Color.FromArgb("#AD1457"),
            Color.FromArgb("#4527A0"),
            Color.FromArgb("#1565C0"),
            Color.FromArgb("#00838F"),
            Color.FromArgb("#2E7D32"),
            Color.FromArgb("#9E9D24"),
            Color.FromArgb("#FF8F00"),
            Color.FromArgb("#D84315"),
            Color.FromArgb("#424242"),

            Color.FromArgb("#b71c1c"),
            Color.FromArgb("#4A148C"),
            Color.FromArgb("#1A237E"),
            Color.FromArgb("#01579B"),
            Color.FromArgb("#004D40"),
            Color.FromArgb("#33691E"),
            Color.FromArgb("#F57F17"),
            Color.FromArgb("#E65100"),
            Color.FromArgb("#3E2723"),

            Color.FromArgb("#263238"),
            Color.FromArgb("#880E4F"),
            Color.FromArgb("#311B92"),
            Color.FromArgb("#0D47A1"),
            Color.FromArgb("#006064"),
            Color.FromArgb("#1B5E20"),
            Color.FromArgb("#827717"),
            Color.FromArgb("#FF6F00"),
            Color.FromArgb("#BF360C"),
            Color.FromArgb("#212121"),
        };

    public static readonly Color[] BrightColors = new[]
    {
            Color.FromArgb("#ff1744"),
            Color.FromArgb("#D500F9"),
            Color.FromArgb("#3D5AFE"),
            Color.FromArgb("#00B0FF"),
            Color.FromArgb("#1DE9B6"),
            Color.FromArgb("#76FF03"),
            Color.FromArgb("#FFEA00"),
            Color.FromArgb("#FF9100"),
            Color.FromArgb("#F50057"),
            Color.FromArgb("#651FFF"),
            Color.FromArgb("#2979FF"),
            Color.FromArgb("#00E5FF"),
            Color.FromArgb("#00E676"),
            Color.FromArgb("#C6FF00"),
            Color.FromArgb("#FFC400"),
            Color.FromArgb("#FF3D00")
        };

    public static readonly List<Color> AllColors = Colors.Union(BrightColors).ToList();

    public static Color BrightColor0 => BrightColors[0];
    public static Color BrightColor1 => BrightColors[1];
    public static Color BrightColor2 => BrightColors[2];
    public static Color BrightColor3 => BrightColors[3];
    public static Color BrightColor4 => BrightColors[4];
    public static Color BrightColor5 => BrightColors[5];
    public static Color BrightColor6 => BrightColors[6];
    public static Color BrightColor7 => BrightColors[7];
    public static Color BrightColor8 => BrightColors[8];
    public static Color BrightColor9 => BrightColors[9];
    public static Color BrightColor10 => BrightColors[10];
    public static Color BrightColor11 => BrightColors[11];
    public static Color BrightColor12 => BrightColors[12];
    public static Color BrightColor13 => BrightColors[13];
    public static Color BrightColor14 => BrightColors[14];
    public static Color BrightColor15 => BrightColors[15];
    public static Color BrightColor16 => BrightColors[16];
    public static Color BrightColor17 => BrightColors[17];
    public static Color BrightColor18 => BrightColors[18];
    public static Color BrightColor19 => BrightColors[19];
}