using System.Reflection;

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
        Opacity = 0.2f;
        Margin = new Thickness(0);
        InputTransparent = true;
        Setup();
        UpdateSource();
    }

    protected virtual void Setup()
    {
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Start;
        HeightRequest = 10;
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
    protected override void Setup()
    {
        FileBase = "gradientv";
        VerticalOptions = LayoutOptions.Fill;
        HorizontalOptions = LayoutOptions.Start;
        WidthRequest = 10;
    }
}