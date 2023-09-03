using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Net.Essentials
{
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

    public class TinyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static Action<Action> RunOnUIAction = a => a();

        PropertyInfo[] properties = null;

        protected readonly static Random Random = new Random();
        public bool RaisePropertyChangeOnUI { get; set; }

        public void RaisePropertyChanged([CallerMemberName] string m = null) =>
            RaisePropertyChanged(RaisePropertyChangeOnUI, m);

        public void RaisePropertyChanged(bool onUI, [CallerMemberName] string m = null)
        {
            void Raise() => OnPropertyChanged(m);
            if (onUI) RunOnUIAction(Raise);
            else Raise();
        }

        void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateProperties(bool forceAll = false)
        {
            if (properties == null)
                properties = GetType().GetProperties();

            RunOnUIAction(() =>
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
            RunOnUIAction(() =>
            {
                foreach (var item in names)
                    RaisePropertyChanged(onUI: false, item);
            });
        }

        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            storage = value;
            RaisePropertyChanged(propertyName);
        }
    }

    public partial class ViewModel : TinyViewModel
    {
        public static Action GlobalBackSignalAction;
        public static Action GlobalPopModalSignalAction;

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

        public virtual void GoBack()
        {
            Task.Run(GoBackAsync);
        }

        public virtual Task GoBackAsync()
        {
            if (GlobalBackSignalAction != null)
                RunOnUIAction(GlobalBackSignalAction);
            return Task.CompletedTask;
        }

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
                RunOnUIAction(GlobalPopModalSignalAction);
            return true;
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
    }

    public class WorkerViewModel : TinyViewModel, IDisposable
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

    public class WorkerViewModelPool : TinyViewModel
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
}