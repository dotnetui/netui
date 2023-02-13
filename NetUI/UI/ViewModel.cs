using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Net.UI;

[AttributeUsage(AttributeTargets.All)]
public class NoUpdateAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.All)]
public class UpdatesAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.All)]
public class ManualUpdateAttribute : Attribute
{
}

public class BindableModel : BindableObject, INotifyPropertyChanged
{
    public static Action GlobalBackSignalAction;
    public static Action GlobalPopModalSignalAction;

    PropertyInfo[] properties = null;

    protected readonly static Random Random = new Random();
    public bool RaisePropertyChangeOnUI { get; set; }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        RaisePropertyChanged(propertyName);
    }

    public void RaisePropertyChanged([CallerMemberName] string m = null) =>
        RaisePropertyChanged(RaisePropertyChangeOnUI, m);

    public void RaisePropertyChanged(bool onUI, [CallerMemberName] string m = null)
    {
        void Raise() => base.OnPropertyChanged(m);
        if (onUI) Dispatcher.Dispatch(Raise);
        else Raise();
    }

    protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        storage = value;
        RaisePropertyChanged(propertyName);
    }

    public void UpdateProperties(bool forceAll = false)
    {
        if (properties == null)
            properties = GetType().GetProperties();

        Dispatcher.Dispatch(() =>
        {
            foreach (var item in properties)
            {
                if (item.GetCustomAttributes(typeof(NoUpdateAttribute), true).Any())
                    continue;
                if (item.GetCustomAttributes(typeof(ManualUpdateAttribute), true).Any() && !forceAll)
                    continue;
                if (item.PropertyType.IsAssignableFrom(typeof(ICommand)) && !item.GetCustomAttributes(typeof(UpdatesAttribute), true).Any())
                    continue;

                RaisePropertyChanged(onUI: false, item.Name);
            }
        });
    }

    public void UpdateProperties(IEnumerable<string> names)
    {
        Dispatcher.Dispatch(() =>
        {
            foreach (var item in names)
                RaisePropertyChanged(onUI: false, item);
        });
    }
}

public partial class ViewModel : BindableModel
{
    public ViewModel() { }

    [NoUpdate] public virtual WorkerViewModelPool Workers { get; set; } = new WorkerViewModelPool();

    public virtual void Refresh()
    {
        Task.Run(RefreshAsync);
    }

    public virtual Task RefreshAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Command RefreshCommand => new Command(Refresh);

    public virtual void GoBack()
    {
        Task.Run(GoBackAsync);
    }

    public virtual Task GoBackAsync()
    {
        if (GlobalBackSignalAction != null) 
            Dispatcher.Dispatch(GlobalBackSignalAction);
        return Task.CompletedTask;
    }

    public virtual Command GoBackCommand => new Command(GoBack);

    protected bool backed = false;
    public virtual bool OnBack()
    {
        if (backed) return true;
        backed = true;
        return OnBackSuccessful();
    }

    public virtual bool OnBackSuccessful()
    {
        if (GlobalPopModalSignalAction != null)
            Dispatcher.Dispatch(GlobalPopModalSignalAction);
        return true;
    }

    public virtual void OnAppeared(ContentPage page)
    {

    }

    /// <summary>
    /// Call from the View to activate the view model
    /// </summary>
    public virtual void OnStart()
    {
        backed = false;
    }

    /// <summary>
    /// Call from the view to deactivate the view model
    /// </summary>
    public virtual void OnStop()
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
        public bool IsMacCatalyst { get; } = DeviceInfo.Platform == DevicePlatform.MacCatalyst;
        public bool IsTvOS { get; } = DeviceInfo.Platform == DevicePlatform.tvOS;
        public bool IsMacOS { get; } = DeviceInfo.Platform == DevicePlatform.macOS;
        public bool IsTizen { get; } = DeviceInfo.Platform == DevicePlatform.Tizen;
        public bool IsWinUI { get; } = DeviceInfo.Platform == DevicePlatform.WinUI;
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

public class WorkerViewModel : BindableModel, IDisposable
{
    public event EventHandler<string> OnLog;
    public WorkerViewModelPool Pool;

    [NoUpdate] public string Tag { get; }

    public virtual bool IsBusy
    {
        get => Status == WorkerStatus.Busy;
        set
        {
            if (value) Status = WorkerStatus.Busy;
            else Status = WorkerStatus.Success;
        }
    }

    [ManualUpdate] public bool IsFailed => Status == WorkerStatus.Fail;
    [ManualUpdate] public bool IsFinished => Status == WorkerStatus.Success;
    [ManualUpdate] public bool IsFirstTime => _isFirstTime;
    [ManualUpdate] public bool IsIdle => !IsBusy;
    [ManualUpdate] public bool IsNotFailed => !IsFailed;
    [ManualUpdate] public bool IsNotFinished => !IsFinished;

    protected WorkerStatus _status = WorkerStatus.Busy;
    protected bool _isFirstTime = true;
    public virtual WorkerStatus Status
    {
        get => _status;
        set
        {
            var hasChanged = _status != value;
            if (hasChanged)
                _status = value;
            if (_status == WorkerStatus.Success)
                _isFirstTime = false;
            if (hasChanged)
                UpdateProperties(true);
        }
    }

    public void ResetFirstTime()
    {
        _isFirstTime = true;
        UpdateProperties();
    }

    public void Log(string message = null, [CallerMemberName] string method = null)
    {
        OnLog?.Invoke(this, $"[{method}] {message ?? "(null)"}");
    }

    public void Dispose()
    {
        if (Pool != null)
            Pool.Remove(this);
    }

    public const string DefaultLoadingText = "Loading...";

    protected string _loadingText = DefaultLoadingText;

    public virtual string LoadingText
    {
        get => _loadingText;
        set
        {
            SetProperty(ref _loadingText, value);
        }
    }

    public const string DefaultErrorText = "Something went wrong";
    protected string _errorText = DefaultErrorText;
    [ManualUpdate]
    public virtual string ErrorText
    {
        get => _errorText;
        set
        {
            _errorText = value;
            RaisePropertyChanged();
        }
    }

    public WorkerViewModel()
    {
    }

    public WorkerViewModel(string tag)
    {
        Tag = tag;
    }
}

public class WorkerViewModelPool : BindableModel
{
    public event EventHandler OnUpdate;

    public ObservableCollection<WorkerViewModel> Items { get; } = new ObservableCollection<WorkerViewModel>();

    public bool IsBusy { get; private set; } = false;

    public WorkerViewModel Add([CallerMemberName] string tag = null)
    {
        var item = new WorkerViewModel(tag);
        Items.Add(item);
        item.PropertyChanged += Item_PropertyChanged;
        Refresh();
        return item;
    }


    public void Remove(string tag = null)
    {
        var item = Find(tag);
        Remove(item);
    }

    public void RemoveLast(string tag = null)
    {
        var item = FindLast(tag);
        Remove(item);
    }

    public void Remove(WorkerViewModel item)
    {
        if (item == null || !Items.Contains(item)) return;
        item.PropertyChanged -= Item_PropertyChanged;
        Items.Remove(item);
        Refresh();
    }

    void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Refresh();
    }

    void Refresh()
    {
        IsBusy = Items.Any(x => x.IsBusy);
        RaisePropertyChanged(nameof(IsBusy));
        OnUpdate?.Invoke(this, null);
    }

    public WorkerViewModel Find(string tag = null)
    {
        return Items.FirstOrDefault(x => x.Tag == tag);
    }

    public WorkerViewModel FindLast(string tag = null)
    {
        return Items.LastOrDefault(x => x.Tag == tag);
    }
}