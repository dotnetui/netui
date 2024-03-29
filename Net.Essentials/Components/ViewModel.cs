﻿using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

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

    [AttributeUsage(AttributeTargets.All)]
    public class PostWorkUpdateAttribute : Attribute
    {
    }

    public class TinyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static Action<Action> RunOnUIAction = a => a();
        public static Func<Func<Task>, Task> RunOnUITask = a => a();

        PropertyInfo[] properties = null;
        HashSet<PropertyInfo> noUpdateProperties = new HashSet<PropertyInfo>();
        HashSet<PropertyInfo> manualUpdateProperties = new HashSet<PropertyInfo>();
        HashSet<PropertyInfo> rigidCommandsProperties = new HashSet<PropertyInfo>();
        HashSet<PropertyInfo> postWorkUpdateProperties = new HashSet<PropertyInfo>();

        protected readonly static Random Random = new Random();
        [Browsable(false)]
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

        void LoadProperties()
        {
            if (properties == null)
            {
                properties = GetType().GetProperties();
                foreach (var item in properties)
                {
                    if (item.GetCustomAttributes(typeof(NoUpdateAttribute), true).Any())
                        noUpdateProperties.Add(item);
                    if (item.GetCustomAttributes(typeof(ManualUpdateAttribute), true).Any())
                        manualUpdateProperties.Add(item);
                    if (item.PropertyType.IsAssignableFrom(typeof(ICommand)) && !item.GetCustomAttributes(typeof(UpdatesAttribute), true).Any())
                        rigidCommandsProperties.Add(item);
                    if (item.GetCustomAttributes(typeof(PostWorkUpdateAttribute), true).Any())
                        postWorkUpdateProperties.Add(item);
                }
            }
        }

        public void UpdateProperties(bool forceAll = false)
        {
            LoadProperties();

            RunOnUIAction(() =>
            {
                foreach (var item in properties)
                {
                    if (noUpdateProperties.Contains(item))
                        continue;
                    if (manualUpdateProperties.Contains(item) && !forceAll)
                        continue;
                    if (postWorkUpdateProperties.Contains(item))
                        continue;

                    RaisePropertyChanged(onUI: false, item.Name);
                }
            });
        }

        public void UpdatePostWorkProperties()
        {
            LoadProperties();
            RunOnUIAction(() =>
            {
                foreach (var item in postWorkUpdateProperties)
                    RaisePropertyChanged(onUI: false, item.Name);
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

        WorkerViewModelPool _workers = null;
        [Browsable(false)]
        [NoUpdate]
        public virtual WorkerViewModelPool Workers
        {
            get
            {
                if (_workers == null)
                {
                    _workers = new WorkerViewModelPool();
                    _workers.OnUpdate += (s, e) => UpdatePostWorkProperties();
                }
                return _workers;
            }
        }

        public ViewModel()
        {
        }

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
            RaisePropertyChangeOnUI = true;
        }

        public WorkerViewModel(string tag) : this()
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
            RunOnUIAction(() => Items.Add(item));
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
            RunOnUIAction(() => Items.Remove(item));
            Refresh();
        }

        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }

        void Refresh()
        {
            IsBusy = Items.Any(x => x.IsBusy);
            RaisePropertyChanged(true, nameof(IsBusy));
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