namespace Net.UI;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SideBar
{
    public SideBar()
    {
        InitializeComponent();
        controlButtons.BindingContext = this;
        UpdateButton();
        UpdateTitleAlignment();
    }

    public double ButtonContainerHeightRequest
    {
        get => (double)GetValue(ButtonContainerHeightRequestProperty);
        set => SetValue(ButtonContainerHeightRequestProperty, value);
    }

    public new bool PadTop
    {
        get => (bool)GetValue(PadTopProperty);
        set => SetValue(PadTopProperty, value);
    }

    public new bool PadBottom
    {
        get => (bool)GetValue(PadBottomProperty);
        set => SetValue(PadBottomProperty, value);
    }

    public double ContentWidthRequest
    {
        get => (double)GetValue(ContentWidthRequestProperty);
        set => SetValue(ContentWidthRequestProperty, value);
    }

    public static readonly BindableProperty ButtonContainerHeightRequestProperty = BindableProperty.Create(
        nameof(ButtonContainerHeightRequest),
        typeof(double),
        typeof(SideBar),
        42.0);

    public static new readonly BindableProperty PadTopProperty = BindableProperty.Create(
       nameof(PadTop),
       typeof(bool),
       typeof(SideBar),
       true);

    public static new readonly BindableProperty PadBottomProperty = BindableProperty.Create(
        nameof(PadBottom),
        typeof(bool),
        typeof(SideBar),
        false);

    public static readonly BindableProperty ContentWidthRequestProperty = BindableProperty.Create(
        propertyName: nameof(ContentWidthRequest),
        returnType: typeof(double),
        declaringType: typeof(SideBar),
        defaultValue: 60.0);

    protected override void UpdateButton()
    {
        UpdateButton(backImage);
    }

    protected override void UpdateTitleAlignment()
    {
        if (lblTitle == null) return;
        if (TitleAlignment == TextAlignment.Center)
        {
            lblTitle.SetValue(Grid.RowProperty, 0);
            lblTitle.SetValue(Grid.RowSpanProperty, 3);

            titleView.SetValue(Grid.RowProperty, 0);
            titleView.SetValue(Grid.RowSpanProperty, 3);
        }
        else
        {
            lblTitle.SetValue(Grid.RowProperty, 1);
            lblTitle.SetValue(Grid.RowSpanProperty, 1);

            titleView.SetValue(Grid.RowProperty, 1);
            titleView.SetValue(Grid.RowSpanProperty, 1);
        }

        lblTitle.HorizontalTextAlignment = TitleAlignment;
    }
}