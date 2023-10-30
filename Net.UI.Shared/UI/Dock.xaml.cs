#if XAMARIN
using Xamarin.Forms;
#else
using View = Microsoft.Maui.IView;
#endif

using Net.Essentials;

namespace Net.UI
{
    [ContentProperty(nameof(Body))]
    public partial class Dock
    {
        public View Body
        {
            get => (View)GetValue(BodyProperty);
            set => SetValue(BodyProperty, value);
        }

        public View Top
        {
            get => (View)GetValue(TopProperty);
            set => SetValue(TopProperty, value);
        }

        public View Bottom
        {
            get => (View)GetValue(BottomProperty);
            set => SetValue(BottomProperty, value);
        }

        public View Left
        {
            get => (View)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        public View Right
        {
            get => (View)GetValue(RightProperty);
            set => SetValue(RightProperty, value);
        }

        public View Overlay
        {
            get => (View)GetValue(OverlayProperty);
            set => SetValue(OverlayProperty, value);
        }

        public bool DropShadows
        {
            get => (bool)GetValue(DropShadowsProperty);
            set => SetValue(DropShadowsProperty, value);
        }

        public static readonly BindableProperty DropShadowsProperty = BindableProperty.Create(
            nameof(DropShadows),
            typeof(bool),
            typeof(Dock));

        public static readonly BindableProperty BodyProperty = BindableProperty.Create(
            nameof(Body),
            typeof(View),
            typeof(Dock));

        public static readonly BindableProperty TopProperty = BindableProperty.Create(
            nameof(Top),
            typeof(View),
            typeof(Dock));

        public static readonly BindableProperty BottomProperty = BindableProperty.Create(
            nameof(Bottom),
            typeof(View),
            typeof(Dock));

        public static readonly BindableProperty LeftProperty = BindableProperty.Create(
            nameof(Left),
            typeof(View),
            typeof(Dock));

        public static readonly BindableProperty RightProperty = BindableProperty.Create(
            nameof(Right),
            typeof(View),
            typeof(Dock));

        public static readonly BindableProperty OverlayProperty = BindableProperty.Create(
            nameof(Overlay),
            typeof(View),
            typeof(Dock));

        public Dock()
        {
            InitializeComponent();
        }
    }
}