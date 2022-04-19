using System.ComponentModel;
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

public partial class ViewModel : BindableObject, INotifyPropertyChanged
{
    protected readonly static Random Random = new Random();

    public virtual bool IsModal { get; set; } = true;

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

    public event EventHandler<string> OnLog;

    public virtual bool IsBusy
    {
        get => Status == ActivityState.Busy;
        set
        {
            if (value) Status = ActivityState.Busy;
            else Status = ActivityState.Success;
        }
    }

    bool _isSideBarOpen = false;
    public virtual bool IsSideBarOpen
    {
        get => _isSideBarOpen;
        set
        {
            _isSideBarOpen = value;
            UpdateProperties();
        }
    }

    public virtual Command ShowSideBarCommand => new Command(() => IsSideBarOpen = true);
    public virtual Command HideSideBarCommand => new Command(() => IsSideBarOpen = false);

    [ManualUpdateAttribute] public bool IsFailed => Status == ActivityState.Fail;
    [ManualUpdateAttribute] public bool IsFinished => Status == ActivityState.Success;
    [ManualUpdateAttribute] public bool IsFirstTime => _isFirstTime;
    [ManualUpdateAttribute] public bool IsIdle => !IsBusy;
    [ManualUpdateAttribute] public bool IsNotFailed => !IsFailed;
    [ManualUpdateAttribute] public bool IsNotFinished => !IsFinished;

    ActivityState _status = ActivityState.Busy;
    bool _isFirstTime = true;
    public ActivityState Status
    {
        get => _status;
        set
        {
            var hasChanged = _status != value;
            if (hasChanged)
                _status = value;
            if (_status == ActivityState.Success)
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

    public virtual void Refresh()
    {
        Task.Run(RefreshAsync);
    }

    public virtual Task RefreshAsync()
    {
        return Task.CompletedTask;
    }

    public void Log(string message = null, [CallerMemberName] string method = null)
    {
        OnLog?.Invoke(this, $"[{method}] {message ?? "(null)"}");
    }

    public ViewModel() { }

    protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        storage = value;
        RaisePropertyChanged(propertyName);
    }

    PropertyInfo[] properties = null;
    public void UpdateProperties(bool forceAll = false)
    {
        if (properties == null)
            properties = GetType().GetProperties();

        Device.BeginInvokeOnMainThread(() =>
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
        Device.BeginInvokeOnMainThread(() =>
        {
            foreach (var item in names)
                RaisePropertyChanged(onUI: false, item);
        });
    }

    public const string DefaultLoadingText = "Loading...";

    string _loadingText = DefaultLoadingText;

    public virtual string LoadingText
    {
        get => _loadingText;
        set
        {
            SetProperty(ref _loadingText, value);
        }
    }

    public const string DefaultErrorText = "Something went wrong";
    string _errorText = DefaultErrorText;
    [ManualUpdateAttribute]
    public virtual string ErrorText
    {
        get => _errorText;
        set
        {
            _errorText = value;
            RaisePropertyChanged();
        }
    }

    public virtual Command RefreshCommand => new Command(Refresh);

    public virtual Command GoBackCommand => new Command(() =>
    {
        Signals.RunOnUI.Signal<Action>(() => OnBack());
    });

    protected bool backed = false;
    public virtual bool OnBack()
    {
        if (backed) return true;
        backed = true;
        return OnBackSuccessful();
    }

    public virtual bool OnBackSuccessful()
    {
        if (IsModal)
        {
            Signals.PopModal.Signal();
        }
        else Signals.ShowFirstPage.Signal();
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

    public bool IsIOS => Device.RuntimePlatform == Device.iOS;
    public bool IsAndroid => Device.RuntimePlatform == Device.Android;

    public bool IsTablet => Device.Idiom == TargetIdiom.Tablet;
    public bool IsPhone => Device.Idiom == TargetIdiom.Phone;
}