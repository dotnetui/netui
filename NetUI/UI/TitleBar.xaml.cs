namespace Net.UI;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TitleBar
{
    public TitleBar()
    {
        BackgroundColor = Color.FromArgb("#333");
        HeightRequest = 60;
        InitializeComponent();
        controlButtons.BindingContext = this;
        contentContainer.PadTop = PadTop;
        contentContainer.PadBottom = PadBottom;
        UpdateButton();
        UpdateTitleAlignment();
    }

    public double ButtonContainerWidthRequest
    {
        get => (double)GetValue(ButtonContainerWidthRequestProperty);
        set => SetValue(ButtonContainerWidthRequestProperty, value);
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

    public double ContentHeightRequest
    {
        get => (double)GetValue(ContentHeightRequestProperty);
        set => SetValue(ContentHeightRequestProperty, value);
    }

    public static readonly BindableProperty ButtonContainerWidthRequestProperty = BindableProperty.Create(
        nameof(ButtonContainerWidthRequest),
        typeof(double),
        typeof(TitleBar),
        42.0);

    public static new readonly BindableProperty PadTopProperty = BindableProperty.Create(
       nameof(PadTop),
       typeof(bool),
       typeof(TitleBar),
       true,
       propertyChanged: (bindable, oldVal, newVal) =>
       {
           if (bindable is TitleBar titleBar && titleBar.contentContainer != null)
               titleBar.contentContainer.PadTop = (bool)newVal;
       });

    public static new readonly BindableProperty PadBottomProperty = BindableProperty.Create(
        nameof(PadBottom),
        typeof(bool),
        typeof(TitleBar),
        false,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titleBar && titleBar.contentContainer != null)
                titleBar.contentContainer.PadBottom = (bool)newVal;
        });

    public static readonly BindableProperty ContentHeightRequestProperty = BindableProperty.Create(
        propertyName: nameof(ContentHeightRequest),
        returnType: typeof(double),
        declaringType: typeof(TitleBarBase),
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
            lblTitle.SetValue(Grid.ColumnProperty, 0);
            lblTitle.SetValue(Grid.ColumnSpanProperty, 3);

            titleView.SetValue(Grid.ColumnProperty, 0);
            titleView.SetValue(Grid.ColumnSpanProperty, 3);
        }
        else
        {
            lblTitle.SetValue(Grid.ColumnProperty, 1);
            lblTitle.SetValue(Grid.ColumnSpanProperty, 1);

            titleView.SetValue(Grid.ColumnProperty, 1);
            titleView.SetValue(Grid.ColumnSpanProperty, 1);
        }

        lblTitle.HorizontalTextAlignment = TitleAlignment;
    }
}