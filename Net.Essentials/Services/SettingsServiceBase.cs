using Net.Essentials;
using System.Threading.Tasks;

namespace Net.Essentials
{
    public class SettingsServiceBase<T> : Singleton<SettingsServiceBase<T>>
        where T : class, new()
    {
        Persistent<T> settings = new Persistent<T>();

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
}