using Realms;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials;

public abstract class RealmTable<TRecord, TModel, TUpdate> 
    where TRecord : RealmObject, IRecord, new ()
    where TModel : class, new ()
    where TUpdate : class, new ()
{
    protected readonly Func<Realm> GetRealm;

    public RealmTable(Func<Realm> getRealm)
    {
        this.GetRealm = getRealm;
    }

    public virtual TModel ToModel(TRecord record)
    {
        return record?.ReturnAs<TModel>();
    }

    public virtual TRecord ToRecord(TModel model)
    {
        return model?.ReturnAs<TRecord>();
    }

    public virtual TRecord CreateRecord(TUpdate update)
    {
        var record = ToRecord(update?.ReturnAs<TModel>());
        if (record != null)
            record.Id = IdExtensions.GenerateId();
        return record;
    }

    public virtual TRecord UpdateRecord(TUpdate update, TRecord record)
    {
        if (record != null && update != null)
            update.Inject(record);
        return record;
    }

    public virtual TModel UpdateModel(TUpdate update, TModel model)
    {
        if (model == null) model = new TModel();
        update?.Inject(model);
        return model;
    }

    public async Task<TModel> GetAsync(string id)
    {
        TModel result = null;
        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                result = ToModel(realm.All<TRecord>()
                    .FirstOrDefault(x => x.Id == id));
            }
        });
        return result;
    }

    public async Task<TModel> AddAsync(TUpdate update)
    {
        TModel result = null;
        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                realm.Write(() =>
                {
                    var record = CreateRecord(update);
                    realm.Add(record);
                    result = ToModel(record);
                });
            }
        });
        return result;
    }

    public async Task<TModel> UpdateAsync(string id, TUpdate update)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (update == null)
            throw new ArgumentNullException(nameof(update));

        TModel result = null;
        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                realm.Write(() =>
                {
                    var record = realm.All<TRecord>()
                        .FirstOrDefault(x => x.Id == id);

                    if (record == null)
                        throw new NotFoundException($"Record {id} not found.");

                    UpdateRecord(update, record);
                    result = ToModel(record);
                });
            }
        });
        return result;
    }


    public async Task<List<TModel>> GetAllAsync()
    {
        List<TModel> results = null;

        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                results = realm.All<TRecord>()
                    .ToList()
                    .Select(x => ToModel(x))
                    .ToList();
            }
        });

        return results;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        bool success = false;

        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                var record = realm.All<TRecord>().FirstOrDefault(x => x.Id == id);

                if (record != null)
                {
                    realm.Write(() =>
                    {
                        realm.Remove(record);
                        success = true;
                    });
                }
            }
        });

        return success;
    }

    public async Task<int> CountAsync(Func<TRecord, bool> predicate = null)
    {
        int count = 0;
        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                var all = realm.All<TRecord>();
                if (predicate == null)
                    count = all.Count();
                else
                    count = all.Count(predicate);
            }
        });
        return count;
    }
    
    public async Task<long> LongCountAsync(Func<TRecord, bool> predicate = null)
    {
        long count = 0;
        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                var all = realm.All<TRecord>();
                if (predicate == null)
                    count = all.LongCount();
                else
                    count = all.LongCount(predicate);
            }
        });
        return count;
    }

    public async Task<List<TModel>> WhereAsync(Func<TRecord, bool> predicate)
    {
        List<TModel> results = null;
        await Task.Run(() =>
        {
            using (var realm = GetRealm())
            {
                var all = realm.All<TRecord>().Where(predicate);
                results = all.ToList().Select(x => ToModel(x)).ToList();
            }
        });
        return results;
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
