using Net.Essentials.Services;
using Net.Internals;

using System.ComponentModel;
using System.Windows.Input;

namespace Net.UI;

public enum TitleBarMainButton
{
    Hide,
    Menu,
    Back,
    Custom
}

[XamlCompilation(XamlCompilationOptions.Compile)]
[ContentProperty("ToolBar")]
public partial class TitleBar
{
    public TitleBar()
    {
        InitializeComponent();
        UpdateButton();
        backImage.InputTransparent = true;

        var tap = new TapGestureRecognizer();
        tap.Tapped += Tap_Tapped;
        controlButtons.GestureRecognizers.Add(tap);

        UpdateToolBar();
        UpdateLeftToolBar();
        UpdateTitleView();
        contentContainer.FixTopPadding = FixTopPadding;
        contentContainer.FixBottomPadding = FixBottomPadding;

        UpdateTitleAlignment();
    }

    private void Tap_Tapped(object sender, EventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) ?? false)
            Command.Execute(CommandParameter);
    }

    public View BackgroundView
    {
        get => (View)GetValue(BackgroundViewProperty);
        set => SetValue(BackgroundViewProperty, value);
    }

    public View ToolBar
    {
        get => (View)GetValue(ToolBarProperty);
        set => SetValue(ToolBarProperty, value);
    }

    public View LeftToolBar
    {
        get => (View)GetValue(LeftToolBarProperty);
        set => SetValue(LeftToolBarProperty, value);
    }

    public View TitleView
    {
        get => (View)GetValue(TitleViewProperty);
        set => SetValue(TitleViewProperty, value);
    }

    public double ContentHeightRequest
    {
        get => (double)GetValue(ContentHeightRequestProperty);
        set => SetValue(ContentHeightRequestProperty, value);
    }

    public Thickness ContentMargin
    {
        get => (Thickness)GetValue(ContentMarginProperty);
        set => SetValue(ContentMarginProperty, value);
    }

    public TitleBarMainButton Button
    {
        get => (TitleBarMainButton)GetValue(ButtonProperty);
        set => SetValue(ButtonProperty, value);
    }

    public ImageSource CustomButtonImage
    {
        get => (ImageSource)GetValue(CustomButtonImageProperty);
        set => SetValue(CustomButtonImageProperty, value);
    }

    public ImageSource BackgroundImage
    {
        get => (ImageSource)GetValue(BackgroundImageProperty);
        set => SetValue(BackgroundImageProperty, value);
    }

    public double BackgroundImageOpacity
    {
        get => (double)GetValue(BackgroundImageOpacityProperty);
        set => SetValue(BackgroundImageOpacityProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public new bool FixTopPadding
    {
        get => (bool)GetValue(FixTopPaddingProperty);
        set => SetValue(FixTopPaddingProperty, value);
    }

    public new bool FixBottomPadding
    {
        get => (bool)GetValue(FixBottomPaddingProperty);
        set => SetValue(FixBottomPaddingProperty, value);
    }

    public TextAlignment TitleAlignment
    {
        get => (TextAlignment)GetValue(TitleAlignmentProperty);
        set => SetValue(TitleAlignmentProperty, value);
    }

    public bool IsDark
    {
        get => (bool)GetValue(IsDarkProperty);
        set => SetValue(IsDarkProperty, value);
    }

    public ControlTemplate TitleViewTemplate
    {
        get => (ControlTemplate)GetValue(TitleViewTemplateProperty);
        set => SetValue(TitleViewTemplateProperty, value);
    }

    public static readonly BindableProperty IsDarkProperty = BindableProperty.Create(
        propertyName: nameof(IsDark),
        returnType: typeof(bool),
        declaringType: typeof(TitleBar),
        defaultValue: false,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titlebar)
                titlebar.UpdateButton();
        });

    public static readonly BindableProperty TitleAlignmentProperty = BindableProperty.Create(
        propertyName: nameof(TitleAlignment),
        returnType: typeof(TextAlignment),
        declaringType: typeof(TitleBar),
        defaultValue: TextAlignment.Start,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titlebar)
                titlebar.UpdateTitleAlignment();
        });

    public static readonly BindableProperty BackgroundViewProperty = BindableProperty.Create(
        propertyName: nameof(BackgroundView),
        returnType: typeof(View),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
        });

    public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create(
        propertyName: nameof(BackgroundImage),
        returnType: typeof(ImageSource),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty ToolBarProperty = BindableProperty.Create(
        propertyName: nameof(ToolBar),
        returnType: typeof(View),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titlebar)
                titlebar.UpdateToolBar();
        });

    public static readonly BindableProperty LeftToolBarProperty = BindableProperty.Create(
        propertyName: nameof(LeftToolBar),
        returnType: typeof(View),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titlebar)
                titlebar.UpdateLeftToolBar();
        });

    public static readonly BindableProperty TitleViewProperty = BindableProperty.Create(
        propertyName: nameof(TitleView),
        returnType: typeof(View),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titlebar)
                titlebar.UpdateTitleView();
        });

    public static readonly BindableProperty ContentHeightRequestProperty = BindableProperty.Create(
        propertyName: nameof(ContentHeightRequest),
        returnType: typeof(double),
        declaringType: typeof(TitleBar),
        defaultValue: 60.0,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty BackgroundImageOpacityProperty = BindableProperty.Create(
        propertyName: nameof(BackgroundImageOpacity),
        returnType: typeof(double),
        declaringType: typeof(TitleBar),
        defaultValue: 1.0,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty ContentMarginProperty = BindableProperty.Create(
        propertyName: nameof(ContentMargin),
        returnType: typeof(Thickness),
        declaringType: typeof(TitleBar),
        defaultValue: default(Thickness),
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty ButtonProperty = BindableProperty.Create(
        propertyName: nameof(Button),
        returnType: typeof(TitleBarMainButton),
        declaringType: typeof(TitleBar),
        defaultValue: TitleBarMainButton.Back,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            (bindable as TitleBar)?.UpdateButton();
        });

    public static readonly BindableProperty CustomButtonImageProperty = BindableProperty.Create(
        propertyName: nameof(CustomButtonImage),
        returnType: typeof(ImageSource),
        declaringType: typeof(TitleBar),
        defaultValue: default(ImageSource),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            (bindable as TitleBar)?.UpdateButton();
        });

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        propertyName: nameof(Command),
        returnType: typeof(ICommand),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        propertyName: nameof(CommandParameter),
        returnType: typeof(object),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        propertyName: nameof(TextColor),
        returnType: typeof(Color),
        declaringType: typeof(TitleBar),
        defaultValue: Color.FromArgb("#FFF"),
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        propertyName: nameof(FontSize),
        returnType: typeof(double),
        declaringType: typeof(TitleBar),
        defaultValue: 16.0,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        propertyName: nameof(FontFamily),
        returnType: typeof(string),
        declaringType: typeof(TitleBar),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty TitleViewTemplateProperty = BindableProperty.Create(
        propertyName: nameof(TitleViewTemplate),
        returnType: typeof(ControlTemplate),
        declaringType: typeof(TitleBar));

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
        propertyName: nameof(FontAttributes),
        returnType: typeof(FontAttributes),
        declaringType: typeof(TitleBar),
        defaultValue: FontAttributes.Bold,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static new readonly BindableProperty FixTopPaddingProperty = BindableProperty.Create(
       nameof(FixTopPadding),
       typeof(bool),
       typeof(TitleBar),
       true,
       propertyChanged: (bindable, oldVal, newVal) =>
       {
           if (bindable is TitleBar titlebar && titlebar.contentContainer != null)
               titlebar.contentContainer.FixTopPadding = (bool)newVal;
       });

    public static new readonly BindableProperty FixBottomPaddingProperty = BindableProperty.Create(
        nameof(FixBottomPadding),
        typeof(bool),
        typeof(TitleBar),
        false,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titlebar && titlebar.contentContainer != null)
                titlebar.contentContainer.FixBottomPadding = (bool)newVal;
        });


    public static ImageSource BackButtonImageOverride = null;
    public static ImageSource MenuButtonImageOverride = null;
    public static Size? MainButtonSizeOverride = null;

    void UpdateButton()
    {
        Dispatcher.Dispatch(() =>
        {
            controlButtons.IsVisible = Button != TitleBarMainButton.Hide && LeftToolBar == null;

            if (Button == TitleBarMainButton.Hide)
                return;

            if (MainButtonSizeOverride != null)
            {
                backImage.WidthRequest = MainButtonSizeOverride.Value.Width;
                backImage.HeightRequest = MainButtonSizeOverride.Value.Height;
            }

            if (Button == TitleBarMainButton.Custom)
            {
                backImage.Source = CustomButtonImage;
                return;
            }

            var showBack = Button == TitleBarMainButton.Back;
            var darkSuffix = IsDark ? "b" : "";

            if (showBack)
                backImage.Source = BackButtonImageOverride ?? EmbeddedResourceManager.Instance.LoadImage($"leftarrow{darkSuffix}.png");
            else
                backImage.Source = MenuButtonImageOverride ?? EmbeddedResourceManager.Instance.LoadImage($"menu{darkSuffix}.png");
        });
    }

    void UpdateToolBar()
    {
        buttons.Content = ToolBar;
    }

    void UpdateLeftToolBar()
    {
        leftbuttons.Content = LeftToolBar;
        leftbuttons.IsVisible = LeftToolBar != null;
        UpdateButton();
    }

    void UpdateTitleView()
    {
        titleView.Content = TitleView;
        lblTitle.IsVisible = TitleView == null;
    }

    void UpdateTitleAlignment()
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