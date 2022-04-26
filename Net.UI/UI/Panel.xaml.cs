namespace Net.UI;

[ContentProperty(nameof(Body))]
public partial class Panel
{
	public IView Body
    {
		get => (IView)GetValue(BodyProperty);
		set => SetValue(BodyProperty, value);
    }

	public IView TopBar
    {
		get => (IView)GetValue(TopBarProperty);
		set => SetValue(TopBarProperty, value);
    }
	
	public IView BottomBar
    {
		get => (IView)GetValue(BottomBarProperty);
		set => SetValue(BottomBarProperty, value);
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

	public static readonly BindableProperty BodyProperty = BindableProperty.Create(
		nameof(Body),
		typeof(IView),
		typeof(Panel));
	
	public static readonly BindableProperty TopBarProperty = BindableProperty.Create(
		nameof(TopBar),
		typeof(IView),
		typeof(Panel));
	
	public static readonly BindableProperty BottomBarProperty = BindableProperty.Create(
		nameof(BottomBar),
		typeof(IView),
		typeof(Panel));
	
	public static readonly BindableProperty OverlayProperty = BindableProperty.Create(
		nameof(Overlay),
		typeof(IView),
		typeof(Panel));
	
	public static readonly BindableProperty DropShadowsProperty = BindableProperty.Create(
		nameof(DropShadows),
		typeof(bool),
		typeof(Panel));

	public Panel()
	{
		InitializeComponent();
	}
}