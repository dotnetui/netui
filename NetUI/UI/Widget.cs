namespace Net.UI;

public class Widget : ContentView
{
    public bool PadTop
    {
        get => (bool)GetValue(PadTopProperty);
        set => SetValue(PadTopProperty, value);
    }

    public bool PadBottom
    {
        get => (bool)GetValue(PadBottomProperty);
        set => SetValue(PadBottomProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty PadTopProperty =
        BindableProperty.Create(
            nameof(PadTop),
            typeof(bool),
            typeof(Widget),
            defaultValue: false);

    public static readonly BindableProperty PadBottomProperty =
        BindableProperty.Create(
            nameof(PadBottom),
            typeof(bool),
            typeof(Widget),
            defaultValue: false);

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(Widget));
}
