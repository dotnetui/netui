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
    Image
}

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TitleBar
{
    public TitleBar()
    {
        InitializeComponent();
        controlButtons.BindingContext = this;
        contentContainer.FixTopPadding = FixTopPadding;
        contentContainer.FixBottomPadding = FixBottomPadding;
        UpdateButton();
        UpdateTitleAlignment();
    }

    public double ButtonContainerWidthRequest
    {
        get => (double)GetValue(ButtonContainerWidthRequestProperty);
        set => SetValue(ButtonContainerWidthRequestProperty, value);
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

    public double ContentHeightRequest
    {
        get => (double)GetValue(ContentHeightRequestProperty);
        set => SetValue(ContentHeightRequestProperty, value);
    }

    public static readonly BindableProperty ButtonContainerWidthRequestProperty = BindableProperty.Create(
        nameof(ButtonContainerWidthRequest),
        typeof(double),
        typeof(TitleBarBase),
        42.0);

    public static new readonly BindableProperty FixTopPaddingProperty = BindableProperty.Create(
       nameof(FixTopPadding),
       typeof(bool),
       typeof(TitleBarBase),
       true,
       propertyChanged: (bindable, oldVal, newVal) =>
       {
           if (bindable is TitleBar titleBar && titleBar.contentContainer != null)
               titleBar.contentContainer.FixTopPadding = (bool)newVal;
       });

    public static new readonly BindableProperty FixBottomPaddingProperty = BindableProperty.Create(
        nameof(FixBottomPadding),
        typeof(bool),
        typeof(TitleBarBase),
        false,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is TitleBar titleBar && titleBar.contentContainer != null)
                titleBar.contentContainer.FixBottomPadding = (bool)newVal;
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