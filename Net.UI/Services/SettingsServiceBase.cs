namespace Net.Essentials.Services;

public abstract class SettingsServiceBase<T> where T : class, new()
{
    volatile bool isSaving = false;
    Persistent<T> settings = new();

    public T Settings => settings.Data;

    public async Task SaveAsync()
    {
        if (isSaving) return;

        isSaving = true;
        await Task.Run(() => Save());
        isSaving = false;
    }

    public void Save()
    {
        if (settings == null)
            settings = new();
        settings.Save();
    }
}
