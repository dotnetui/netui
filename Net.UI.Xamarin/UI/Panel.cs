using Xamarin.Forms;

namespace Net.UI
{
    [ContentProperty(nameof(Content))]
    public class Panel : Grid
    {
        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly BindableProperty ContentProperty = BindableProperty.Create(
            nameof(Content),
            typeof(View),
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
                Dispatcher.BeginInvokeOnMainThread(Children.Clear);
            else if (Content != null)
            {
                if (Children.Count > 0 &&
                    !Children.Contains(Content))
                    Dispatcher.BeginInvokeOnMainThread(Children.Clear);
                if (!Children.Contains(Content))
                    Dispatcher.BeginInvokeOnMainThread(() => Children.Add(Content));
            }
        }
    }
}