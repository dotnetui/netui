using Net.Internals;

namespace Net.UI;

public class ShadowBar : Image
{
    public bool IsFlipped
    {
        get => (bool)GetValue(IsFlippedProperty);
        set => SetValue(IsFlippedProperty, value);
    }

    public static readonly BindableProperty IsFlippedProperty = BindableProperty.Create(
        nameof(IsFlipped),
        typeof(bool),
        typeof(ShadowBar),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is ShadowBar shadow) shadow.UpdateSource();
        });

    protected string FileBase = "gradienth";

    public ShadowBar()
    {
        Aspect = Aspect.Fill;
        HorizontalOptions = LayoutOptions.Fill;
        HeightRequest = 10;
        Opacity = 0.2f;
        Margin = new Thickness(0);
        VerticalOptions = LayoutOptions.Start;
        InputTransparent = true;
        UpdateSource();
    }

    protected void UpdateSource()
    {
        var file = FileBase;
        if (IsFlipped) file += "2";
        Source = ImageSource.FromResource($"Net.UI.{file}.png", typeof(ShadowBar).GetTypeInfo().Assembly);
    }
}

public class VerticalShadowBar : ShadowBar
{
    public VerticalShadowBar()
    {
        FileBase = "gradientv";
        VerticalOptions = LayoutOptions.Fill;
        WidthRequest = 10;
        HeightRequest = -1;
        HorizontalOptions = LayoutOptions.Start;
        UpdateSource();
    }
}