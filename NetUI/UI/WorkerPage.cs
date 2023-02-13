namespace Net.UI;

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