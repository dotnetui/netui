using Net.Internals;

namespace Net.UI;

public class HorizontalShadow : Image
{
    public bool IsFlipped
    {
        get => (bool)GetValue(IsFlippedProperty);
        set => SetValue(IsFlippedProperty, value);
    }

    public static readonly BindableProperty IsFlippedProperty = BindableProperty.Create(
        nameof(IsFlipped),
        typeof(bool),
        typeof(HorizontalShadow),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is HorizontalShadow shadow) shadow.UpdateSource();
        });

    protected string FileBase = "gradienth";

    public HorizontalShadow()
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
        Source = ImageSource.FromResource($"Net.UI.{file}.png", typeof(HorizontalShadow).GetTypeInfo().Assembly);
    }
}

public class VerticalShadow : HorizontalShadow
{
    public VerticalShadow()
    {
        FileBase = "gradientv";
        VerticalOptions = LayoutOptions.Fill;
        WidthRequest = 10;
        HeightRequest = -1;
        HorizontalOptions = LayoutOptions.Start;
        UpdateSource();
    }
}