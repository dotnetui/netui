namespace Net.UI;

public class HorizontalShadow : Image
{
    public HorizontalShadow()
    {
        Source = ImageSource.FromResource("Net.UI.gradienth.png", typeof(HorizontalShadow).GetTypeInfo().Assembly);
        Aspect = Aspect.Fill;
        HorizontalOptions = LayoutOptions.Fill;
        HeightRequest = 10;
        Opacity = 0.2f;
        Margin = new Thickness(0);
        VerticalOptions = LayoutOptions.Start;
        InputTransparent = true;
    }
}

public class HorizontalShadow2 : HorizontalShadow
{
    public HorizontalShadow2()
    {
        VerticalOptions = LayoutOptions.End;
        Source = ImageSource.FromResource("Net.UI.gradienth2.png", typeof(HorizontalShadow2).GetTypeInfo().Assembly);
    }
}

public class VerticalShadow : HorizontalShadow
{
    public VerticalShadow()
    {
        VerticalOptions = LayoutOptions.Fill;
        WidthRequest = 10;
        HeightRequest = -1;
        HorizontalOptions = LayoutOptions.Start;
        Source = ImageSource.FromResource("Net.UI.gradientv.png", typeof(VerticalShadow).GetTypeInfo().Assembly);
    }
}

public class VerticalShadow2 : VerticalShadow
{
    public VerticalShadow2()
    {
        HorizontalOptions = LayoutOptions.End;
        Source = ImageSource.FromResource("Net.UI.gradientv2.png", typeof(VerticalShadow2).GetTypeInfo().Assembly);
    }
}

public class HorizontalLine : BoxView
{
    public HorizontalLine()
    {
        HorizontalOptions = LayoutOptions.Fill;
        HeightRequest = 1;
        BackgroundColor = Color.FromArgb("#FFF");
    }
}

public class VerticalLine : BoxView
{
    public VerticalLine()
    {
        VerticalOptions = LayoutOptions.Fill;
        WidthRequest = 1;
        BackgroundColor = Color.FromArgb("#FFF");
    }
}