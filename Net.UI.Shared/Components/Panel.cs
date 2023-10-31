using Net.Essentials;

using System;
using System.Windows.Input;

#if XAMARIN
using Xamarin.Forms;
using Xamarin.Essentials;

using IView = Xamarin.Forms.View;
#endif

namespace Net.UI
{
    [ContentProperty(nameof(Content))]
    public class Panel : Grid
    {
        public IView Content
        {
            get => (IView)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly BindableProperty ContentProperty = BindableProperty.Create(
            nameof(Content),
            typeof(IView),
            typeof(Panel),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                if (bindable is Panel panel)
                    panel.UpdateContent();
            });

        public Panel()
        {
            UpdateContent();
        }

        void UpdateContent()
        {
            if (Content == null && Children.Count > 0)
                MainThread.BeginInvokeOnMainThread(Children.Clear);
            else if (Content != null)
            {
                if (Children.Count > 0 &&
                    !Children.Contains(Content))
                    MainThread.BeginInvokeOnMainThread(Children.Clear);
                if (!Children.Contains(Content))
                    MainThread.BeginInvokeOnMainThread(() => Children.Add(Content));
            }
        }
    }
}