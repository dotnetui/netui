using Net.Essentials;
using Net.Essentials.Systems;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace Net.UI
{
    public class Application2 : Application
    {
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

                XViewModel.RunOnUIAction = RunOnUI;

                HookSignals();
                InitializeSystems();
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

        void HookSignals()
        {
            Signals.PopModal.Subscribe(this, PopModal);
            Signals.RunOnUI.Subscribe<Action>(this, RunOnUI);
            Signals.ShowFirstPage.Subscribe(this, ShowFirstPage);
            Signals.ShowPage.Subscribe<Page>(this, page => ShowPage(page));
            Signals.ShowModalPage.Subscribe<Page>(this, ShowModalPage);
        }

        public virtual async void ShowSplashPage()
        {
            if (SplashTasks.Count > 0)
            {
                MainPage = GetSplashPage();
                await Task.WhenAll(SplashTasks.Select(x => x()));
                SplashTasks.Clear();
            }

            ShowFirstPage();
        }

        public virtual Page GetSplashPage() => new ContentPage
        {
            Content = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true
            }
        };

        public virtual Page GetFirstPage() => new ContentPage
        {
            Content = new Label
            {
                Text = "Override Net.UI's FirstPage",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };

        public virtual async Task ShowFirstPageAsync()
        {
            await ShowPageAsync(GetFirstPage());
        }

        public void ShowFirstPage() =>
            RunOnUI(async () => await ShowFirstPageAsync());

        public virtual async Task ShowPageAsync(Page page, bool isModal = false)
        {
            if (isModal && MainPage != null)
                await MainPage.Navigation.PushModalAsync(page);
            else
                MainPage = page;
        }

        public void ShowPage(Page page, bool isModal = false) =>
            RunOnUI(async () => await ShowPageAsync(page, isModal));

        public virtual void RunOnUI(Action a)
        {
            MainThread.BeginInvokeOnMainThread(a);
        }

        public virtual void PopModal()
        {
            if ((MainPage?.Navigation?.ModalStack?.Count ?? 0) > 0)
                MainPage.Navigation.PopModalAsync();
        }

        public virtual async Task PopModalAsync()
        {
            if ((MainPage?.Navigation?.ModalStack?.Count ?? 0) > 0)
                await MainPage.Navigation.PopModalAsync();
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
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await MainPage?.DisplayAlert(title, message, button);
            });
        }

        public virtual async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            bool result = false;
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                result = await MainPage?.DisplayAlert(title, message, accept, cancel);
            });
            return result;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            IsResumed = true;
            Signals.AppStart.Signal();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            IsResumed = false;
            Signals.AppSleep.Signal();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            IsResumed = true;
            Signals.AppResume.Signal();
        }
    }
}