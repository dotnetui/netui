namespace Net.UI;

public class Shadow : Image
{
    public bool IsFlipped
    {
        get => (bool)GetValue(IsFlippedProperty);
        set => SetValue(IsFlippedProperty, value);
    }

    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly BindableProperty IsFlippedProperty = BindableProperty.Create(
        nameof(IsFlipped),
        typeof(bool),
        typeof(Shadow),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Shadow shadow) shadow.Update();
        });

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(Shadow),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Shadow shadow) shadow.Update();
        });

    const double Size = 10;

    public Shadow()
    {
        BackgroundColor = Color.FromArgb("#F00");
        Aspect = Aspect.Fill;
        Opacity = 0.2f;
        Margin = new Thickness(0);
        InputTransparent = true;
        Update();
    }

    void Update()
    {
        string source = "gradienth";
        if (Orientation == StackOrientation.Horizontal)
        {
            HorizontalOptions = LayoutOptions.Fill;
            WidthRequest = -1;
            HeightRequest = Size;
            VerticalOptions = LayoutOptions.Start;
            if (IsFlipped) source += "2";
        }
        else
        {
            HeightRequest = -1;
            WidthRequest = Size;
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Fill;
            source = IsFlipped ? "gradientv2" : "gradientv";
        }

        Source = ImageSource.FromResource($"Net.UI.{source}.png", typeof(Shadow).GetTypeInfo().Assembly);
    }
}

public class Line : BoxView
{
    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(Line),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Line line) line.Update();
        });

    public Line()
    {
        HorizontalOptions = LayoutOptions.Fill;
        HeightRequest = 1;
        WidthRequest = 1;
        BackgroundColor = Color.FromArgb("#FFF");
        Update();
    }

    void Update()
    {
        if (Orientation == StackOrientation.Horizontal)
        {
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Start;
        }
        else
        {
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Fill;
        }
    }
}