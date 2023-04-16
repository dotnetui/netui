using Realms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Essentials
{
    public abstract class SyncableRealmTable<TInterface, TRecord, TModel, TUpdate> :
            RealmTable<TInterface, TRecord, TModel, TUpdate>
            where TInterface : IRecord, ISyncable
            where TRecord : RealmObject, TInterface, new()
            where TModel : class, TInterface, new()
            where TUpdate : class, new()
    {
        readonly BenchmarkService benchmarkService;

        public static DateTimeOffset DeletedDateThreshold = new DateTimeOffset(1000, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public SyncableRealmTable(Func<Realm> getRealm) : base(getRealm)
        {
        }

        public override TRecord CreateRecord(TUpdate update)
        {
            var record = base.CreateRecord(update);
            if (record != null)
            {
                record.CreateTimestamp = DateTimeOffset.Now;
                UpdateRecord(record);
            }
            return record;
        }

        public override TRecord UpdateRecord(TUpdate update, TRecord record)
        {
            record = base.UpdateRecord(update, record);
            UpdateRecord(record);
            return record;
        }

        protected void UpdateRecord(TRecord record)
        {
            if (record != null)
            {
                record.UpdateTimestamp = DateTimeOffset.Now;
                record.SyncTimestamp = default;
            }
        }

        public async Task MarkDeletedAsync(TModel model)
        {
            if (model == null) return;
            await MarkDeletedAsync(model.Id);
        }

        public async Task<TModel> MarkDeletedAsync(string id)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (var realm = GetRealm())
                    {
                        var record = realm.All<TRecord>().FirstOrDefault(x => x.Id == id);
                        if (record != null)
                            realm.Write(() =>
                            {
                                record.DeleteTimestamp = DateTimeOffset.Now;
                                UpdateRecord(record);
                            });
                    }
                });

                if (EnableCaching)
                    MakeCacheDirty(id);
                return await GetAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TModel>> GetAvailableAsync()
        {
            using (benchmarkService.StartBenchmark($"{typeof(TModel)}.GetAvailableAsync()"))
                return await WhereAsync(x => x.DeleteTimestamp < DeletedDateThreshold);
        }

        public async Task SyncAsync(List<TModel> cloudModels, DateTimeOffset syncTimestamp)
        {
            if (cloudModels == null) return;

            using (benchmarkService.StartBenchmark($"{typeof(TModel)}.SyncAsync()"))
            {
                bool isDirty = false;
                foreach (var cloudModel in cloudModels)
                {
                    await Task.Run(() =>
                    {
                        using (var realm = GetRealm())
                        {
                            var record = realm.All<TRecord>().FirstOrDefault(x => x.Id == cloudModel.Id);
                            realm.Write(() =>
                            {
                                isDirty = true;
                                if (record == null)
                                {
                                    record = cloudModel.ReturnAs<TRecord>();
                                    record.SyncTimestamp = syncTimestamp;
                                    realm.Add(record);
                                }
                                else
                                {
                                    cloudModel.Inject(record);
                                    record.SyncTimestamp = syncTimestamp;
                                }
                            });
                        }
                    });
                }
                if (isDirty && EnableCaching)
                    MakeCacheDirty();
            }
        }
    }
}