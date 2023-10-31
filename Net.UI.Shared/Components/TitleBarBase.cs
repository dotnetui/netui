using Net.Internals;

using System.ComponentModel;
using System.Windows.Input;
using System;

#if XAMARIN
using Xamarin.Forms;
using Xamarin.Essentials;
using IView = Xamarin.Forms.View;
using TypeConverter = Xamarin.Forms.TypeConverterAttribute;
#endif

namespace Net.UI
{
    public enum TitleBarMainButton
    {
        Hide,
        Menu,
        Back,
        Image
    }

    [ContentProperty(nameof(Body))]
    public abstract class TitleBarBase : Widget
    {
        public static ImageSource BackButtonImageOverride = null;
        public static ImageSource MenuButtonImageOverride = null;

        public IView Behind
        {
            get => (IView)GetValue(BehindProperty);
            set => SetValue(BehindProperty, value);
        }

        public IView Trail
        {
            get => (IView)GetValue(TrailProperty);
            set => SetValue(TrailProperty, value);
        }

        public IView Lead
        {
            get => (IView)GetValue(LeadProperty);
            set => SetValue(LeadProperty, value);
        }

        public IView Body
        {
            get => (IView)GetValue(BodyProperty);
            set => SetValue(BodyProperty, value);
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

        public ControlTemplate BodyTemplate
        {
            get => (ControlTemplate)GetValue(BodyTemplateProperty);
            set => SetValue(BodyTemplateProperty, value);
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

        public static readonly BindableProperty BehindProperty = BindableProperty.Create(
            propertyName: nameof(Behind),
            returnType: typeof(IView),
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

        public static readonly BindableProperty TrailProperty = BindableProperty.Create(
            propertyName: nameof(Trail),
            returnType: typeof(IView),
            declaringType: typeof(TitleBarBase),
            defaultValue: null);

        public static readonly BindableProperty LeadProperty = BindableProperty.Create(
            propertyName: nameof(Lead),
            returnType: typeof(IView),
            declaringType: typeof(TitleBarBase),
            defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                if (bindable is TitleBarBase titlebar)
                    titlebar.UpdateButton();
            });

        public static readonly BindableProperty BodyProperty = BindableProperty.Create(
            propertyName: nameof(Body),
            returnType: typeof(IView),
            declaringType: typeof(TitleBarBase),
            defaultValue: null);

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
            defaultValue: Palette.FromHex("#FFF"),
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

        public static readonly BindableProperty BodyTemplateProperty = BindableProperty.Create(
            propertyName: nameof(BodyTemplate),
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
            MainThread.BeginInvokeOnMainThread(() =>
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
}