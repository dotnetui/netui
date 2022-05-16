namespace Net.Essentials;

public abstract class SettingsServiceBase<T> where T : class, new()
{
    Persistent<T> settings = new();

    public T Data => settings.Data;

    public async Task SaveAsync()
    {
        await settings.SaveAsync();
    }

    public void SaveWithLock()
    {
        settings.SaveWithLock();
    }

    public void QueueSave()
    {
        settings.QueueSave();
    }
}
