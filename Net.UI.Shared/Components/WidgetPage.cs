using Net.Essentials;
using Net.Essentials.Systems;

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#if XAMARIN
using Xamarin.Forms;
#if __IOS__
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using VisualElement = Xamarin.Forms.VisualElement;
#endif
#endif

namespace Net.UI
{

    public enum UIStatusBarStyles
    {
        Default = 0,
        LightContent = 1,
        BlackOpaque = 2,
        DarkContent = 3
    }

    public class WidgetPage : ContentPage
    {
        bool isStarted = false;
        WeakReference<ViewModel> lastContext = null;

        public event EventHandler OnAppeared;
        public event EventHandler OnDisappeared;

        public Action PlatformUpdate;

        public bool PadTop
        {
            get => (bool)GetValue(PadTopProperty);
            set => SetValue(PadTopProperty, value);
        }

        public bool PadBottom
        {
            get => (bool)GetValue(PadBottomProperty);
            set => SetValue(PadBottomProperty, value);
        }

        public UIStatusBarStyles UIStatusBarStyle
        {
            get => (UIStatusBarStyles)GetValue(UIStatusBarStyleProperty);
            set => SetValue(UIStatusBarStyleProperty, value);
        }

        public Color AndroidStatusBarColor
        {
            get => (Color)GetValue(AndroidStatusBarColorProperty);
            set => SetValue(AndroidStatusBarColorProperty, value);
        }

        public bool AndroidTranslucentStatus
        {
            get => (bool)GetValue(AndroidTranslucentStatusProperty);
            set => SetValue(AndroidTranslucentStatusProperty, value);
        }

        public bool AndroidLayoutInScreen
        {
            get => (bool)GetValue(AndroidLayoutInScreenProperty);
            set => SetValue(AndroidLayoutInScreenProperty, value);
        }

        public bool UIStatusBarHidden
        {
            get => (bool)GetValue(UIStatusBarHiddenProperty);
            set => SetValue(UIStatusBarHiddenProperty, value);
        }

        public bool UIStatusBarAnimated
        {
            get => (bool)GetValue(UIStatusBarAnimatedProperty);
            set => SetValue(UIStatusBarAnimatedProperty, value);
        }

        public bool? KeepScreenOn
        {
            get => (bool?)GetValue(KeepScreenOnProperty);
            set => SetValue(KeepScreenOnProperty, value);
        }

        public static BindableProperty PadTopProperty = BindableProperty.Create(
            nameof(PadTop),
            typeof(bool),
            typeof(WidgetPage),
            false,
            BindingMode.OneWay,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                (bindable as WidgetPage).AdjustPadding();
            });

        public static BindableProperty PadBottomProperty = BindableProperty.Create(
            nameof(PadBottom),
            typeof(bool),
            typeof(WidgetPage),
            false,
            BindingMode.OneWay,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                (bindable as WidgetPage).AdjustPadding();
            });

        public static BindableProperty UIStatusBarStyleProperty = BindableProperty.Create(
            nameof(UIStatusBarStyle),
            typeof(UIStatusBarStyles),
            typeof(WidgetPage),
            UIStatusBarStyles.LightContent,
            BindingMode.OneWay,
            propertyChanged: UpdateSettings);

        public static BindableProperty UIStatusBarHiddenProperty = BindableProperty.Create(
            nameof(UIStatusBarHidden),
            typeof(bool),
            typeof(WidgetPage),
            false,
            BindingMode.OneWay,
            propertyChanged: UpdateSettings);

        public static BindableProperty UIStatusBarAnimatedProperty = BindableProperty.Create(
            nameof(UIStatusBarAnimated),
            typeof(bool),
            typeof(WidgetPage),
            true,
            BindingMode.OneWay,
            propertyChanged: UpdateSettings);

        public static BindableProperty AndroidStatusBarColorProperty = BindableProperty.Create(
            nameof(AndroidStatusBarColor),
            typeof(Color),
            typeof(WidgetPage),
            Palette.Black,
            BindingMode.OneWay,
            propertyChanged: UpdateSettings);

        public static BindableProperty AndroidTranslucentStatusProperty = BindableProperty.Create(
            nameof(AndroidTranslucentStatus),
            typeof(bool),
            typeof(WidgetPage),
            true,
            BindingMode.OneWay,
            propertyChanged: UpdateSettings);

        public static BindableProperty AndroidLayoutInScreenProperty = BindableProperty.Create(
            nameof(AndroidLayoutInScreen),
            typeof(bool),
            typeof(WidgetPage),
            true,
            BindingMode.OneWay,
            propertyChanged: UpdateSettings);

        public static BindableProperty KeepScreenOnProperty = BindableProperty.Create(
            nameof(KeepScreenOn),
            typeof(bool?),
            typeof(WidgetPage),
            (bool?)null,
            BindingMode.OneWay,
            propertyChanged: UpdateSettings);

        static void UpdateSettings(BindableObject bindable, object oldVal, object newVal)
        {
            (bindable as WidgetPage)?.PlatformUpdate?.Invoke();
        }

        public WidgetPage()
        {
#if XAMARIN && __IOS__
            this.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetModalPresentationStyle(Xamarin.Forms.PlatformConfiguration.iOSSpecific.UIModalPresentationStyle.FullScreen);
#endif
            AdjustPadding();
        }

        IEnumerable<Widget> GetAliveChildren()
        {
            if (Content is VisualElement ve)
            {
                return ve
                    .GetAllChildren()
                    .Where(x => x is Widget)
                    .Select(x => (Widget)x);
            }

            return new Widget[] { };
        }

        protected override void OnAppearing()
        {
            OnAppeared?.Invoke(this, null);

            base.OnAppearing();

            AdjustPadding();
            PlatformUpdate?.Invoke();
            var mvm = BindingContext as ViewModel2;
            if (BindingContext is ViewModel vm)
            {
                if (!isStarted)
                {
                    if (lastContext != null && lastContext.TryGetTarget(out var lastVm))
                    {
                        lastVm.OnStop();
                    }
                    isStarted = true;
                    mvm?.OnStart();
                    lastContext = new WeakReference<ViewModel>(vm);
                }

                mvm?.OnAppeared(this);
            }
        }

        Thickness? originalPadding;

        public void AdjustPadding()
        {
            if (originalPadding == null)
            {
                originalPadding = Padding;
            }

            var pageMargin = NotchSystem.Instance.GetPageMargin();
            var top = PadTop ? pageMargin.Top : originalPadding.Value.Top;
            var bottom = PadBottom ? pageMargin.Bottom : originalPadding.Value.Bottom;
            Padding = new Thickness(
                originalPadding.Value.Left,
                top,
                originalPadding.Value.Right,
                bottom);

            foreach (var child in GetAliveChildren())
                child.AdjustPadding();
        }

        protected override void OnDisappearing()
        {
            OnDisappeared?.Invoke(this, null);

            if (isStarted && BindingContext != null)
            {
                isStarted = false;
                if (BindingContext is ViewModel2 mvm)
                    mvm.OnStop();
                lastContext = null;
            }

            base.OnDisappearing();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Content))
            {
                foreach (var item in GetAliveChildren())
                {
                    item.HookToAlivePage(this);
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            ViewModel lastVm = default;
            if (lastContext != null && lastContext.TryGetTarget(out lastVm))
            {
                if (lastVm is ViewModel2 mvm)
                    mvm.OnUnbind(this);
            }

            if (BindingContext is ViewModel vm)
            {
                if (vm is ViewModel2 mvm)
                    mvm.OnBind(this);
                if (!isStarted)
                {
                    if (lastVm is ViewModel2 lastMvm)
                        lastMvm?.OnStop();
                    isStarted = true;
                    vm.OnStart();
                    lastContext = new WeakReference<ViewModel>(vm);
                }
            }

            base.OnBindingContextChanged();
        }

        protected override bool OnBackButtonPressed()
        {
            if (BindingContext is ViewModel vm)
            {
                if (vm.OnBack()) return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}