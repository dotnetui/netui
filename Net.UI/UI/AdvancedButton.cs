﻿#if __IOS__
using UIKit;
#elif __ANDROID__
using Android.Views;
using Google.Android.Material.Button;

using Microsoft.Maui.Handlers;

using TextAlignment = Microsoft.Maui.TextAlignment;
#endif

namespace Net.UI;

public class AdvancedButton : Button
{
    public new event EventHandler Pressed;
    public new event EventHandler Released;

    public TextAlignment TextAlignment
    {
        get => (TextAlignment)GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    public static readonly BindableProperty TextAlignmentProperty =
        BindableProperty.Create(nameof(TextAlignment),
        typeof(TextAlignment),
        typeof(AdvancedButton),
        TextAlignment.Center,
        BindingMode.OneWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is AdvancedButton button)
                button.UpdateAlignment();
        });

    public virtual void OnPressed()
    {
        Pressed?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnReleased()
    {
        Released?.Invoke(this, EventArgs.Empty);
    }

#if __IOS__
    public new event EventHandler Clicked;
    public virtual void OnClicked()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
#endif

    public AdvancedButton()
    {
        this.Loaded += AdvancedButton_Loaded;
    }

    private void AdvancedButton_Loaded(object sender, EventArgs e)
    {
#if __IOS__
        var uiButton = Handler as UIButton;

        uiButton.TouchDown += UiButton_TouchDown;
        uiButton.TouchUpOutside += UiButton_TouchUpOutside;
        uiButton.TouchCancel += UiButton_TouchCancel;
        uiButton.TouchUpInside += UiButton_TouchUpInside;
#elif __ANDROID__
        if (Handler is ViewHandler vh)
        {
            if (vh.PlatformView is MaterialButton mButton)
            {
                mButton.Touch += (s, args) =>
                {
                    if (args.Event.Action == MotionEventActions.Down)
                        OnPressed();
                    else if (args.Event.Action == MotionEventActions.Up)
                        OnReleased();
                    else if (args.Event.Action == MotionEventActions.Cancel)
                        OnReleased();
                    args.Handled = false;
                };
            } else if (vh.PlatformView is Android.Widget.Button wButton)
            {
                wButton.SetBackgroundColor(Android.Graphics.Color.Transparent);
                wButton.Touch += (s, args) =>
                {
                    if (args.Event.Action == MotionEventActions.Down)
                        OnPressed();
                    else if (args.Event.Action == MotionEventActions.Up)
                        OnReleased();
                    else if (args.Event.Action == MotionEventActions.Cancel)
                        OnReleased();
                    args.Handled = false;
                };
            }
        }
#endif
        UpdateAlignment();
    }

#if __IOS__
    void UiButton_TouchUpInside(object sender, EventArgs e)
    {
        OnReleased();
        OnClicked();
    }

    void UiButton_TouchCancel(object sender, EventArgs e)
    {
        OnReleased();
    }

    void UiButton_TouchUpOutside(object sender, EventArgs e)
    {
        OnReleased();
    }

    void UiButton_TouchDown(object sender, EventArgs e)
    {
        OnPressed();
    }
#endif

    void UpdateAlignment()
    {
#if __IOS__
        if (Handler is UIButton uiButton)
        {
            uiButton.HorizontalAlignment = TextAlignment switch
            {
                TextAlignment.Start => UIControlContentHorizontalAlignment.Left,
                TextAlignment.Center => UIControlContentHorizontalAlignment.Center,
                _ => UIControlContentHorizontalAlignment.Right
            };
        }
#elif __ANDROID__
        if (Handler is ViewHandler vh)
        {
            if (vh.PlatformView is Android.Widget.Button wButton)
            {
                wButton.TextAlignment = TextAlignment switch
                {
                    TextAlignment.Center => Android.Views.TextAlignment.Center,
                    TextAlignment.End => Android.Views.TextAlignment.ViewEnd,
                    _ => Android.Views.TextAlignment.ViewStart
                };
            }
            else if (vh.PlatformView is MaterialButton mButton)
            {
                mButton.TextAlignment = TextAlignment switch
                {
                    TextAlignment.Center => Android.Views.TextAlignment.Center,
                    TextAlignment.End => Android.Views.TextAlignment.ViewEnd,
                    _ => Android.Views.TextAlignment.ViewStart
                };
            }
        }
#endif
    }
}