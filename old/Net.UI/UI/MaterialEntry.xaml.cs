namespace Net.UI;

public partial class MaterialishEntry
{
	public string Placeholder { get; set; } = "Placeholder";
	public string Text
    {
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
    }

	public Color PlaceholderColor { get; set; } = Palette.LightGray;
	public Color TextColor { get; set; } = Palette.Black;
	public Color FocusedBackgroundColor { get; set; } = Palette.LightGray;
	public Color AccentColor { get; set; } = Palette.LightGray;

	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text),
		typeof(string),
		typeof(MaterialishEntry));

	public MaterialishEntry()
	{
		InitializeComponent();
		grid.BindingContext = this;
	}

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
		Focus();
    }
	public new bool IsFocused => entry.IsFocused;

	public new void Focus()
    {
		if (IsFocused) return;
		entry.Focus();
    }

	public new void Unfocus()
    {
		if (IsFocused)
			entry.Unfocus();
    }
}