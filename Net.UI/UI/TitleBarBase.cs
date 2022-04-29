using Net.Internals;

using System.ComponentModel;
using System.Windows.Input;

namespace Net.UI;

public class TitleBarBase : Widget
{
    public static ImageSource BackButtonImageOverride = null;
    public static ImageSource MenuButtonImageOverride = null;

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

    public ImageSource ButtonImage
    {
        get => (ImageSource)GetValue(ButtonImageProperty);
        set => SetValue(ButtonImageProperty, value);
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

    public double ButtonHeightRequest
    {
        get => (double)GetValue(ButtonHeightRequestProperty);
        set => SetValue(ButtonHeightRequestProperty, value);
    }

    public double ButtonWidthRequest
    {
        get => (double)GetValue(ButtonWidthRequestProperty);
        set => SetValue(ButtonWidthRequestProperty, value);
    }

    public Thickness ButtonMargin
    {
        get => (Thickness)GetValue(ButtonMarginProperty);
        set => SetValue(ButtonMarginProperty, value);
    }

    public Aspect ButtonAspect
    {
        get => (Aspect)GetValue(ButtonAspectProperty);
        set => SetValue(ButtonAspectProperty, value);
    }

    public static readonly BindableProperty IsDarkProperty = BindableProperty.Create(
        propertyName: nameof(IsDark),
        returnType: typeof(bool),
        declaringType: typeof(TitleBarBase),
        defaultValue: false,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBarBase titlebar)
                titlebar.UpdateButton();
        });

    public static readonly BindableProperty TitleAlignmentProperty = BindableProperty.Create(
        propertyName: nameof(TitleAlignment),
        returnType: typeof(TextAlignment),
        declaringType: typeof(TitleBarBase),
        defaultValue: TextAlignment.Start,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBarBase titlebar)
                titlebar.UpdateTitleAlignment();
        });

    public static readonly BindableProperty BackgroundViewProperty = BindableProperty.Create(
        propertyName: nameof(BackgroundView),
        returnType: typeof(View),
        declaringType: typeof(TitleBarBase),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
        });

    public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create(
        propertyName: nameof(BackgroundImage),
        returnType: typeof(ImageSource),
        declaringType: typeof(TitleBarBase),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty ToolBarProperty = BindableProperty.Create(
        propertyName: nameof(ToolBar),
        returnType: typeof(View),
        declaringType: typeof(TitleBarBase),
        defaultValue: null);

    public static readonly BindableProperty LeftToolBarProperty = BindableProperty.Create(
        propertyName: nameof(LeftToolBar),
        returnType: typeof(View),
        declaringType: typeof(TitleBarBase),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBarBase titlebar)
                titlebar.UpdateButton();
        });

    public static readonly BindableProperty TitleViewProperty = BindableProperty.Create(
        propertyName: nameof(TitleView),
        returnType: typeof(View),
        declaringType: typeof(TitleBarBase),
        defaultValue: null);

    public static readonly BindableProperty ContentHeightRequestProperty = BindableProperty.Create(
        propertyName: nameof(ContentHeightRequest),
        returnType: typeof(double),
        declaringType: typeof(TitleBarBase),
        defaultValue: 60.0);

    public static readonly BindableProperty BackgroundImageOpacityProperty = BindableProperty.Create(
        propertyName: nameof(BackgroundImageOpacity),
        returnType: typeof(double),
        declaringType: typeof(TitleBarBase),
        defaultValue: 1.0);

    public static readonly BindableProperty ContentMarginProperty = BindableProperty.Create(
        propertyName: nameof(ContentMargin),
        returnType: typeof(Thickness),
        declaringType: typeof(TitleBarBase),
        defaultValue: default(Thickness));

    public static readonly BindableProperty ButtonProperty = BindableProperty.Create(
        propertyName: nameof(Button),
        returnType: typeof(TitleBarMainButton),
        declaringType: typeof(TitleBarBase),
        defaultValue: TitleBarMainButton.Back,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            (bindable as TitleBar)?.UpdateButton();
        });

    public static readonly BindableProperty ButtonImageProperty = BindableProperty.Create(
        propertyName: nameof(ButtonImage),
        returnType: typeof(ImageSource),
        declaringType: typeof(TitleBarBase),
        defaultValue: default(ImageSource),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            (bindable as TitleBar)?.UpdateButton();
        });

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        propertyName: nameof(Command),
        returnType: typeof(ICommand),
        declaringType: typeof(TitleBarBase),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        propertyName: nameof(CommandParameter),
        returnType: typeof(object),
        declaringType: typeof(TitleBarBase),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        propertyName: nameof(TextColor),
        returnType: typeof(Color),
        declaringType: typeof(TitleBarBase),
        defaultValue: Color.FromArgb("#FFF"),
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        propertyName: nameof(FontSize),
        returnType: typeof(double),
        declaringType: typeof(TitleBarBase),
        defaultValue: 16.0,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        propertyName: nameof(FontFamily),
        returnType: typeof(string),
        declaringType: typeof(TitleBarBase),
        defaultValue: null,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty TitleViewTemplateProperty = BindableProperty.Create(
        propertyName: nameof(TitleViewTemplate),
        returnType: typeof(ControlTemplate),
        declaringType: typeof(TitleBarBase));

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
        propertyName: nameof(FontAttributes),
        returnType: typeof(FontAttributes),
        declaringType: typeof(TitleBarBase),
        defaultValue: FontAttributes.Bold,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty ButtonHeightRequestProperty = BindableProperty.Create(
        nameof(ButtonHeightRequest),
        typeof(double),
        typeof(TitleBarBase),
        24.0);

    public static readonly BindableProperty ButtonWidthRequestProperty = BindableProperty.Create(
        nameof(ButtonWidthRequest),
        typeof(double),
        typeof(TitleBarBase),
        24.0);

    public static readonly BindableProperty ButtonMarginProperty = BindableProperty.Create(
        nameof(ButtonMargin),
        typeof(Thickness),
        typeof(TitleBarBase),
        new Thickness(12, 0, 6, 0));

    public static readonly BindableProperty ButtonAspectProperty = BindableProperty.Create(
        nameof(ButtonAspect),
        typeof(Aspect),
        typeof(TitleBarBase),
        Aspect.AspectFit);

    protected virtual void UpdateButton()
    {
    }

    protected void UpdateButton(Image backImage)
    {
        Dispatcher.Dispatch(() =>
        {
            if (Button == TitleBarMainButton.Hide)
                return;

            if (Button == TitleBarMainButton.Image)
            {
                backImage.Source = ButtonImage;
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

    protected virtual void UpdateTitleAlignment()
    {
    }

    protected void Tap_Tapped(object sender, EventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) ?? false)
            Command.Execute(CommandParameter);
    }
}
