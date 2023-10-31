#if __IOS__
using UIKit;
#elif __ANDROID__
using Android.Views;
using Google.Android.Material.Button;
#endif

#if XAMARIN
using System;
using Xamarin.Forms;
using TextAlignment = Xamarin.Forms.TextAlignment;
#else
using TextAlignment = Microsoft.Maui.TextAlignment;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#endif

namespace Net.UI
{

    public class Button2 : Button
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
            typeof(Button2),
            TextAlignment.Center,
            BindingMode.OneWay,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
#if !XAMARIN
                if (bindable is Button2 button)
                    button.UpdateAlignment();
#endif
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

#if !XAMARIN
        public Button2()
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
#elif ANDROID
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
#elif WINDOWS
        if (Handler is ButtonHandler bh && bh.PlatformView is MauiButton button)
        {
            button.PointerPressed += (s, e) => 
                OnPressed();
            button.PointerReleased += (s, e) => 
                OnReleased();
            button.PointerCanceled += (s, e) => 
                OnReleased();
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
#endif
    }
}