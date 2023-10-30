using System.Collections.Generic;
using System.Linq;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.UI
{
    public static class Palette
    {
        static Color FromHex(string hex)
        {
#if XAMARIN
            return Color.FromHex(hex);
#else
            return Color.FromArgb(hex);
#endif
        }

        public static Color Default { get; } = new Color();
        public static Color Black { get; } = FromHex("#000");
        public static Color White { get; } = FromHex("#FFF");
        public static Color Transparent { get; } = FromHex("#00000000");
        public static Color LightGray { get; } = FromHex("#D3D3D3");
        public static Color Silver { get; } = FromHex("#C0C0C0");
        public static Color DarkGray { get; } = FromHex("#A9A9A9");
        public static Color Gray { get; } = FromHex("#808080");
        public static Color DimGray { get; } = FromHex("#696969");

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
            FromHex("#c62828"),
            FromHex("#6A1B9A"),
            FromHex("#283593"),
            FromHex("#0277BD"),
            FromHex("#00695C"),
            FromHex("#558B2F"),
            FromHex("#F9A825"),
            FromHex("#EF6C00"),
            FromHex("#4E342E"),
            FromHex("#37474F"),
            FromHex("#AD1457"),
            FromHex("#4527A0"),
            FromHex("#1565C0"),
            FromHex("#00838F"),
            FromHex("#2E7D32"),
            FromHex("#9E9D24"),
            FromHex("#FF8F00"),
            FromHex("#D84315"),
            FromHex("#424242"),

            FromHex("#b71c1c"),
            FromHex("#4A148C"),
            FromHex("#1A237E"),
            FromHex("#01579B"),
            FromHex("#004D40"),
            FromHex("#33691E"),
            FromHex("#F57F17"),
            FromHex("#E65100"),
            FromHex("#3E2723"),

            FromHex("#263238"),
            FromHex("#880E4F"),
            FromHex("#311B92"),
            FromHex("#0D47A1"),
            FromHex("#006064"),
            FromHex("#1B5E20"),
            FromHex("#827717"),
            FromHex("#FF6F00"),
            FromHex("#BF360C"),
            FromHex("#212121"),
        };

        public static readonly Color[] BrightColors = new[]
        {
            FromHex("#ff1744"),
            FromHex("#D500F9"),
            FromHex("#3D5AFE"),
            FromHex("#00B0FF"),
            FromHex("#1DE9B6"),
            FromHex("#76FF03"),
            FromHex("#FFEA00"),
            FromHex("#FF9100"),
            FromHex("#F50057"),
            FromHex("#651FFF"),
            FromHex("#2979FF"),
            FromHex("#00E5FF"),
            FromHex("#00E676"),
            FromHex("#C6FF00"),
            FromHex("#FFC400"),
            FromHex("#FF3D00")
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
}