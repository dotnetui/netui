using Net.Essentials;

using System.ComponentModel;
using System.Windows.Input;

#if XAMARIN
using Xamarin.Forms;
using Xamarin.Essentials;
#endif

namespace Net.UI
{
    public partial class XViewModel : ViewModel
    {
        public XViewModel() { }

        public virtual Command RefreshCommand => new Command(Refresh);
        public virtual Command GoBackCommand => new Command(GoBack);

        public virtual void OnAppeared(ContentPage page)
        {

        }

        public virtual void OnBind(BindableObject bindable)
        {

        }

        public virtual void OnUnbind(BindableObject bindable)
        {

        }

        static readonly DeviceFlags _device = new DeviceFlags();
        public DeviceFlags Device => _device;
        public virtual IDrawerViewModel Drawer { get; protected set; } = new DrawerViewModel();

        public class DeviceFlags
        {
            public bool IsIOS { get; } = DeviceInfo.Platform == DevicePlatform.iOS;
            public bool IsAndroid { get; } = DeviceInfo.Platform == DevicePlatform.Android;
            public bool IsTvOS { get; } = DeviceInfo.Platform == DevicePlatform.tvOS;
            public bool IsMacOS { get; } = DeviceInfo.Platform == DevicePlatform.macOS;
            public bool IsTizen { get; } = DeviceInfo.Platform == DevicePlatform.Tizen;
#if XAMARIN
            public bool IsWinUI { get; } = DeviceInfo.Platform == DevicePlatform.UWP;
#else
            public bool IsWinUI { get; } = DeviceInfo.Platform == DevicePlatform.WinUI;
#endif
            public bool IsWatchOS { get; } = DeviceInfo.Platform == DevicePlatform.watchOS;

            public bool IsDesktop { get; } = DeviceInfo.Idiom == DeviceIdiom.Desktop;
            public bool IsWatch { get; } = DeviceInfo.Idiom == DeviceIdiom.Watch;
            public bool IsPhone { get; } = DeviceInfo.Idiom == DeviceIdiom.Phone;
            public bool IsTablet { get; } = DeviceInfo.Idiom == DeviceIdiom.Tablet;
            public bool IsTV { get; } = DeviceInfo.Idiom == DeviceIdiom.TV;
        }
    }

    public interface IDrawerViewModel : INotifyPropertyChanged
    {
        bool IsOpen { get; set; }
        ICommand OpenCommand { get; }
        ICommand CloseCommand { get; }
        ICommand ToggleCommand { get; }
    }

    public class DrawerViewModel : BindableObject, IDrawerViewModel
    {
        bool _isOpen = false;
        public virtual bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                OnPropertyChanged();
            }
        }

        public virtual ICommand OpenCommand => new Command(() => IsOpen = true);
        public virtual ICommand CloseCommand => new Command(() => IsOpen = false);
        public virtual ICommand ToggleCommand => new Command(() => IsOpen = !IsOpen);
    }
}