using Realms;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Net.Essentials
{
    public class RealmDriver
    {
        public readonly string Path;
        public readonly ulong SchemaVersion;

        public Func<string, string> MapPathFunc = i => i;

        public virtual RealmConfiguration Configuration => new RealmConfiguration(MapPathFunc(Path))
        {
            SchemaVersion = SchemaVersion,
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

        public async Task ImportAsync(string path)
        {
            await Task.Run(() =>
            {
                if (!File.Exists(path))
                    throw new ApplicationException($"Cannot import {path}: File does not exist");

                var config = Configuration;
                using (var realm = Realm.GetInstance(config))
                {
                    if (realm == null)
                        throw new ApplicationException($"Cannot import {path}: Invalid database");
                    // Good.
                }

                if (File.Exists(Path))
                    File.Delete(Path);
                File.Copy(path, Path);
            });
        }
    }
}