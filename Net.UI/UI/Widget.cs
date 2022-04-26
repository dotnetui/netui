namespace Net.UI;

public class Widget : ContentView
{
    public bool FixTopPadding
    {
        get => (bool)GetValue(FixTopPaddingProperty);
        set => SetValue(FixTopPaddingProperty, value);
    }

    public bool FixBottomPadding
    {
        get => (bool)GetValue(FixBottomPaddingProperty);
        set => SetValue(FixBottomPaddingProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty FixTopPaddingProperty =
        BindableProperty.Create(
            nameof(FixTopPadding),
            typeof(bool),
            typeof(Widget),
            defaultValue: false);

    public static readonly BindableProperty FixBottomPaddingProperty =
        BindableProperty.Create(
            nameof(FixBottomPadding),
            typeof(bool),
            typeof(Widget),
            defaultValue: false);

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(Widget));
}
