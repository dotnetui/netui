using Net.Essentials;
using Net.Essentials.Systems;

#if XAMARIN
using Xamarin.Forms;
using Xamarin.Essentials;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
#endif

namespace Net.UI
{
    public class Application2 : Application
    {
        public event EventHandler Slept;
        public event EventHandler Resumed;
        public event EventHandler Started;

        readonly ContainerService _containerService;
        protected readonly List<Func<Task>> SplashTasks = new List<Func<Task>>();
        public static Application2 Instance => ContainerService.Instance.Get<Application2>();
        public virtual bool IsResumed { get; set; }

        public Application2() : base()
        {
            try
            {
                _containerService = ContainerService.Instance;
                _containerService.Set(this);

                TinyViewModel.RunOnUIAction = RunOnUI;
                TinyViewModel.RunOnUITask = RunOnUIAsync;

                HookSignals();
                InitializeSystems();
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                    SplashTasks.Add(UpdatePageMarginAsync);
                ShowSplashPage();
            }
            catch (Exception ex)
            {
                throw new AggregateException("Application2 Construction Exception", ex);
            }
        }

        public virtual void InitializeSystems()
        {

        }

        public virtual void HookSignals()
        {
            Signals.PopModal.Subscribe(this, PopModal);
            Signals.RunOnUI.Subscribe<Action>(this, RunOnUI);
            Signals.ShowFirstPage.Subscribe(this, ShowFirstPage);
            Signals.ShowPage.Subscribe<Page>(this, page => ShowPage(page));
            Signals.ShowModalPage.Subscribe<Page>(this, ShowModalPage);
        }

        public virtual async void ShowSplashPage(bool proceedToFirstPageWhenDone = true)
        {
            await ShowSplashPageAsync(proceedToFirstPageWhenDone);
        }

        public virtual async Task ShowSplashPageAsync(bool proceedToFirstPageWhenDone = true)
        {
            await RunOnUIAsync(async () =>
            {
                if (SplashTasks.Count > 0 || !proceedToFirstPageWhenDone) 
                {
                    MainPage = await GetSplashPageAsync();
                    await Task.WhenAll(SplashTasks.Select(x => x()));
                    SplashTasks.Clear();
                }

                if (proceedToFirstPageWhenDone)
                    ShowFirstPage();
            });
        }

        protected virtual async Task UpdatePageMarginAsync()
        {
            while (!NotchSystem.Instance.HasWindowInformation)
            {
                NotchSystem.Instance.GetPageMargin();
                await Task.Delay(50);
            }
        }

        public virtual Task<Page> GetSplashPageAsync() => Task.FromResult((Page)new ContentPage
        {
            Content = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true
            }
        });

        public virtual Task<Page> GetFirstPageAsync() => Task.FromResult((Page)new ContentPage
        {
            Content = new Label
            {
                Text = "Override Net.UI's FirstPage",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        });

        [Obsolete("Use GetSplashPageAsync instead. This method does a hard .GetAwaiter().GetResult() on the Async version.")]
        public void GetSplashPage() => GetSplashPageAsync().GetAwaiter().GetResult();

        [Obsolete("Use GetFirstPageAsync instead. This method does a hard .GetAwaiter().GetResult() on the Async version.")]
        public void GetFirstPage() => GetFirstPageAsync().GetAwaiter().GetResult();

        public virtual async Task ShowFirstPageAsync()
        {
            await ShowPageAsync(await GetFirstPageAsync());
        }

        public void ShowFirstPage() =>
            RunOnUI(async () => await ShowFirstPageAsync());

        public virtual async Task ShowPageAsync(Page page, bool isModal = false)
        {
            await RunOnUIAsync(async () =>
            {
                if (isModal && MainPage != null)
                    await MainPage.Navigation.PushModalAsync(page);
                else
                    MainPage = page;
            });
        }

        public void ShowPage(Page page, bool isModal = false) =>
            RunOnUI(async () => await ShowPageAsync(page, isModal));

        public virtual void RunOnUI(Action a)
        {
            MainThread.BeginInvokeOnMainThread(a);
        }

        public virtual Task RunOnUIAsync(Func<Task> t)
        {
            return MainThread.InvokeOnMainThreadAsync(t);
        }

        public virtual void PopModal()
        {
            RunOnUI(() =>
            {
                if ((MainPage?.Navigation?.ModalStack?.Count ?? 0) > 0)
                    MainPage.Navigation.PopModalAsync();
            });
        }

        public virtual async Task PopModalAsync()
        {
            await RunOnUIAsync(async () =>
            {
                if ((MainPage?.Navigation?.ModalStack?.Count ?? 0) > 0)
                    await MainPage.Navigation.PopModalAsync();
            });
        }

        public virtual async Task ShowModalPageAsync(Page page) =>
            await ShowPageAsync(page, true);

        public virtual void ShowModalPage(Page page) =>
            ShowPage(page, true);

        public virtual void DisplayAlert(string title, string message, string button)
        {
            RunOnUI(() =>
            {
                MainPage?.DisplayAlert(title, message, button);
            });
        }

        public virtual async Task DisplayAlertAsync(string title, string message, string button)
        {
            await RunOnUIAsync(async () =>
            {
                await MainPage?.DisplayAlert(title, message, button);
            });
        }

        public virtual async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            bool result = false;
            await RunOnUIAsync(async () =>
            {
                result = await MainPage?.DisplayAlert(title, message, accept, cancel);
            });
            return result;
        }

        public virtual async Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            if (MainPage == null)
            {
                throw new ApplicationException("Application2 Exception: MainPage is null, cannot DisplayActionSheetAsync");
            }
            string result = null;
            await RunOnUIAsync(async () =>
            {
                result = await MainPage?.DisplayActionSheet(title, cancel, destruction, buttons);
            });
            return result;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            IsResumed = true;
            Signals.AppStart.Signal();
            Started?.Invoke(this, null);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            IsResumed = false;
            Signals.AppSleep.Signal();
            Slept?.Invoke(this, null);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            IsResumed = true;
            Signals.AppResume.Signal();
            Resumed?.Invoke(this, null);
        }
    }
}