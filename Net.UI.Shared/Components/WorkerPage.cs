using System.Threading.Tasks;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.UI
{
    public class WorkerPage : ContentPage
    {
        readonly Task task;

        public WorkerPage(Task task)
        {
            this.task = task;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(() => task);
        }
    }
}