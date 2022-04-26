﻿using System.ComponentModel;
using System.Windows.Input;

namespace Net.UI;

[Flags]
public enum IconAppearances
{
    Image = 1,
    Text = 2,
    ImageText = Image | Text,
    ImageTextStack = Image | Text | 4
}

public enum IconOrientation
{
    Up = 0,
    Down = 180,
    Right = -90,
    Left = 90
}

public class Icon : Grid
{
    Image image;
    Label label;

    public Icon()
    {
        Build();
    }

    void Build()
    {
        Children.Clear();
        RowDefinitions.Clear();
        image = null;
        label = null;

        if (Appearance.HasFlag(IconAppearances.Image))
        {
            image = new Image
            {
                HorizontalOptions = ImageWidthRequest < 0 ? LayoutOptions.Fill : LayoutOptions.Center,
                VerticalOptions = ImageHeightRequest < 0 ? LayoutOptions.Fill : LayoutOptions.Center,
                Aspect = Aspect.AspectFit,
                InputTransparent = true
            };

            Children.Add(image);

            image.BindingContext = this;
            image.SetBinding(Image.SourceProperty, nameof(Source));
            image.InputTransparent = true;
            image.Margin = ImageMargin;

            image.SetBinding(Image.HeightRequestProperty, nameof(ImageHeightRequest));
            image.SetBinding(Image.WidthRequestProperty, nameof(ImageWidthRequest));
        }

        if (Appearance.HasFlag(IconAppearances.Text))
        {
            label = new Label
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                MaxLines = 1,
                LineBreakMode = LineBreakMode.NoWrap,
                InputTransparent = true
            };

            Children.Add(label);

            label.BindingContext = this;
            label.SetBinding(Label.TextProperty, nameof(Text));
            label.SetBinding(Label.TextColorProperty, nameof(TextColor));
            label.SetBinding(Label.FontSizeProperty, nameof(FontSize));
            label.SetBinding(Label.FontFamilyProperty, nameof(FontFamily));
            label.SetBinding(Label.FontAttributesProperty, nameof(FontAttributes));
            label.SetBinding(Label.HorizontalTextAlignmentProperty, nameof(HorizontalTextAlignment));
            label.SetBinding(Label.VerticalTextAlignmentProperty, nameof(VerticalTextAlignment));
        }

        var button = new Button2
        {
            BindingContext = this,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };

        button.Pressed += Button_Pressed;
        button.Released += Button_Released;
        button.Clicked += Button_Clicked;

        Children.Add(button);

        if (Appearance == IconAppearances.ImageText)
        {
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            RowDefinitions.Add(new RowDefinition { Height = TextHeight });

            button.SetValue(Grid.RowProperty, 0);
            button.SetValue(Grid.RowSpanProperty, 2);

            image.SetValue(Grid.RowProperty, 0);
            label.SetValue(Grid.RowProperty, 1);

            //image.VerticalOptions = LayoutOptions.EndAndExpand;
        }
        else if (Appearance == IconAppearances.ImageTextStack)
        {
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition { Height = TextHeight });
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            button.SetValue(Grid.RowProperty, 0);
            button.SetValue(Grid.RowSpanProperty, 4);

            image.SetValue(Grid.RowProperty, 1);
            label.SetValue(Grid.RowProperty, 2);
        }

        AdjustOrientation();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) ?? false)
            Command.Execute(CommandParameter);
    }

    private void Button_Released(object sender, EventArgs e)
    {
        if (ReleasedCommand?.CanExecute(ReleasedCommandParameter) ?? false)
            ReleasedCommand.Execute(ReleasedCommandParameter);
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        if (PressedCommand?.CanExecute(PressedCommandParameter) ?? false)
            PressedCommand.Execute(PressedCommandParameter);
    }

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
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

    public ICommand PressedCommand
    {
        get => (ICommand)GetValue(PressedCommandProperty);
        set => SetValue(PressedCommandProperty, value);
    }

    public object PressedCommandParameter
    {
        get => GetValue(PressedCommandParameterProperty);
        set => SetValue(PressedCommandParameterProperty, value);
    }

    public ICommand ReleasedCommand
    {
        get => (ICommand)GetValue(ReleasedCommandProperty);
        set => SetValue(ReleasedCommandProperty, value);
    }

    public object ReleasedCommandParameter
    {
        get => GetValue(ReleasedCommandParameterProperty);
        set => SetValue(ReleasedCommandParameterProperty, value);
    }

    public Thickness ImageMargin
    {
        get => (Thickness)GetValue(ImageMarginProperty);
        set => SetValue(ImageMarginProperty, value);
    }

    public IconAppearances Appearance
    {
        get => (IconAppearances)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
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

    public GridLength TextHeight
    {
        get => (GridLength)GetValue(TextHeightProperty);
        set => SetValue(TextHeightProperty, value);
    }

    public double ImageWidthRequest
    {
        get => (double)GetValue(ImageWidthRequestProperty);
        set => SetValue(ImageWidthRequestProperty, value);
    }

    public double ImageHeightRequest
    {
        get => (double)GetValue(ImageHeightRequestProperty);
        set => SetValue(ImageHeightRequestProperty, value);
    }

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public IconOrientation Orientation
    {
        get => (IconOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
        propertyName: nameof(Orientation),
        returnType: typeof(IconOrientation),
        declaringType: typeof(Icon),
        defaultValue: IconOrientation.Up,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Icon butt)
                butt.AdjustOrientation();
        });

    public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(
        propertyName: nameof(VerticalTextAlignment),
        returnType: typeof(TextAlignment),
        declaringType: typeof(Icon),
        defaultValue: TextAlignment.Start);

    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
        propertyName: nameof(HorizontalTextAlignment),
        returnType: typeof(TextAlignment),
        declaringType: typeof(Icon),
        defaultValue: TextAlignment.Center);

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        propertyName: nameof(Source),
        returnType: typeof(ImageSource),
        declaringType: typeof(Icon),
        defaultValue: null);

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(Icon),
        null,
        BindingMode.OneWay);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(Icon),
        null,
        BindingMode.OneWay);

    public static readonly BindableProperty PressedCommandProperty = BindableProperty.Create(
        nameof(PressedCommand),
        typeof(ICommand),
        typeof(Icon),
        null,
        BindingMode.OneWay);

    public static readonly BindableProperty PressedCommandParameterProperty = BindableProperty.Create(
        nameof(PressedCommandParameter),
        typeof(object),
        typeof(Icon),
        null,
        BindingMode.OneWay);

    public static readonly BindableProperty ReleasedCommandProperty = BindableProperty.Create(
        nameof(ReleasedCommand),
        typeof(ICommand),
        typeof(Icon),
        null,
        BindingMode.OneWay);

    public static readonly BindableProperty ReleasedCommandParameterProperty = BindableProperty.Create(
        nameof(ReleasedCommandParameter),
        typeof(object),
        typeof(Icon),
        null,
        BindingMode.OneWay);

    public static readonly BindableProperty ImageMarginProperty = BindableProperty.Create(
        nameof(ImageMargin),
        typeof(Thickness),
        typeof(Icon),
        new Thickness(0, 0),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Icon butt && butt.image != null)
                butt.image.Margin = (Thickness)newVal;
        });

    public static readonly BindableProperty AppearanceProperty = BindableProperty.Create(
        nameof(Appearance),
        typeof(IconAppearances),
        typeof(Icon),
        IconAppearances.Image,
        BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Icon butt)
                butt.Build();
        });

    public static readonly BindableProperty TextHeightProperty = BindableProperty.Create(
        nameof(TextHeight),
        typeof(GridLength),
        typeof(Icon),
        GridLength.Auto,
        BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Icon butt)
            {
                butt.Build();
            }
        });

    public static readonly BindableProperty ImageHeightRequestProperty = BindableProperty.Create(
        nameof(ImageHeightRequest),
        typeof(double),
        typeof(Icon),
        -1.0,
        BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Icon butt)
            {
                butt.Build();
            }
        });

    public static readonly BindableProperty ImageWidthRequestProperty = BindableProperty.Create(
        nameof(ImageWidthRequest),
        typeof(double),
        typeof(Icon),
        -1.0,
        BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is Icon butt)
            {
                butt.Build();
            }
        });

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        propertyName: nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(Icon),
        defaultValue: "",
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        propertyName: nameof(TextColor),
        returnType: typeof(Color),
        declaringType: typeof(Icon),
        defaultValue: Color.FromArgb("#FFF"),
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        propertyName: nameof(FontSize),
        returnType: typeof(double),
        declaringType: typeof(Icon),
        defaultValue: 16.0,
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        propertyName: nameof(FontFamily),
        returnType: typeof(string),
        declaringType: typeof(Icon),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
        propertyName: nameof(FontAttributes),
        returnType: typeof(FontAttributes),
        declaringType: typeof(Icon),
        defaultValue: FontAttributes.Bold,
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {

        });

    void AdjustOrientation()
    {
        if (Appearance == IconAppearances.ImageTextStack || Appearance == IconAppearances.ImageText)
        {
            Rotation = (double)Orientation;
            image.Rotation = 0;
            label.Rotation = 0;
        }
        else
        {
            Rotation = 0;
            if (image != null)
                image.Rotation = (double)Orientation;
            if (label != null)
                label.Rotation = (double)Orientation;
        }
    }
}