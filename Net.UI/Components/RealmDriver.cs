using Realms;
namespace Net.Essentials;

public class RealmDriver
{
    public readonly string Path;
    public readonly ulong SchemaVersion;

    public virtual RealmConfiguration Configuration => new RealmConfiguration(Path)
    {
        SchemaVersion = SchemaVersion
    };

    public RealmDriver(ulong schemaVersion, string path = null)
    {
        this.Path = path;
        this.SchemaVersion = schemaVersion;
    }

    public Realm GetRealm()
    {
        return Realm.GetInstance(Configuration);
    }

    public async Task ZapAsync()
    {
        await Task.Run(() => Realm.DeleteRealm(Configuration));
    }

    public async Task CompactAsync()
    {
        await Task.Run(() => Realm.Compact(Configuration));
    }
}