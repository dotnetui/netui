namespace Net.UI;

[ContentProperty(nameof(Body))]
public partial class Dock
{
    public IView Body
    {
        get => (IView)GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    public IView Top
    {
        get => (IView)GetValue(TopProperty);
        set => SetValue(TopProperty, value);
    }

    public IView Bottom
    {
        get => (IView)GetValue(BottomProperty);
        set => SetValue(BottomProperty, value);
    }

    public IView Left
    {
        get => (IView)GetValue(LeftProperty);
        set => SetValue(LeftProperty, value);
    }

    public IView Right
    {
        get => (IView)GetValue(RightProperty);
        set => SetValue(RightProperty, value);
    }

    public IView Overlay
    {
        get => (IView)GetValue(OverlayProperty);
        set => SetValue(OverlayProperty, value);
    }

    public bool DropShadows
    {
        get => (bool)GetValue(DropShadowsProperty);
        set => SetValue(DropShadowsProperty, value);
    }

    public static readonly BindableProperty DropShadowsProperty = BindableProperty.Create(
        nameof(DropShadows),
        typeof(bool),
        typeof(Dock));

    public static readonly BindableProperty BodyProperty = BindableProperty.Create(
        nameof(Body),
        typeof(IView),
        typeof(Dock));

    public static readonly BindableProperty TopProperty = BindableProperty.Create(
        nameof(Top),
        typeof(IView),
        typeof(Dock));

    public static readonly BindableProperty BottomProperty = BindableProperty.Create(
        nameof(Bottom),
        typeof(IView),
        typeof(Dock));

    public static readonly BindableProperty LeftProperty = BindableProperty.Create(
        nameof(Left),
        typeof(IView),
        typeof(Dock));

    public static readonly BindableProperty RightProperty = BindableProperty.Create(
        nameof(Right),
        typeof(IView),
        typeof(Dock));

    public static readonly BindableProperty OverlayProperty = BindableProperty.Create(
        nameof(Overlay),
        typeof(IView),
        typeof(Dock));

    public Dock()
    {
        InitializeComponent();
    }
}