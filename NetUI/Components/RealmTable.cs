using Realms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Net.Essentials;

public abstract class RealmTable<TInterface, TRecord, TModel, TUpdate>
    where TInterface : IRecord
    where TRecord : RealmObject, TInterface, new()
    where TModel : class, TInterface, new()
    where TUpdate : class, new()
{
    readonly BenchmarkService benchmarkService;

    protected readonly Func<Realm> GetRealm;
    protected Func<TRecord, bool> AvailabilityCriteriaFunc;

    public bool EnableCaching;
    public bool CacheAllBeforeOps;
    public bool AvoidConcurrentCacheAll = true;

    readonly ConcurrentDictionary<string, TModel> cache = new ConcurrentDictionary<string, TModel>();
    bool cacheHasAll = false;
    volatile bool isCachingAll = false;

    public ConcurrentDictionary<string, TModel> GetCache() => cache;

    public void MakeCacheDirty(string id)
    {
        cache.TryRemove(id, out _);
        cacheHasAll = false;
    }

    public void MakeCacheDirty()
    {
        cacheHasAll = false;
    }

    public void UpdateCache(TModel model)
    {
        cache[model.Id] = model;
    }

    public void LoadAllToCache(List<TModel> models)
    {
        foreach (var item in models)
            cache[item.Id] = item;
        cacheHasAll = true;
    }

    public void ClearCache()
    {
        cache.Clear();
        cacheHasAll = false;
    }

    public RealmTable(Func<Realm> getRealm)
    {
        benchmarkService = BenchmarkService.Instance;
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
        await CacheAllIfNeededAsync();
        {
            if (string.IsNullOrWhiteSpace(id))
                return default;

            TModel result = null;
            if (EnableCaching)
                using (benchmarkService.StartBenchmark($"{typeof(TModel)}.GetAsync(Cache)"))
                    cache.TryGetValue(id, out result);
            if (result == null)
            {
                using (benchmarkService.StartBenchmark($"{typeof(TModel)}.GetAsync(Realm)"))
                    await Task.Run(() =>
                    {
                        using (var realm = GetRealm())
                        {
                            result = ToModel(
                                realm.All<TRecord>().FirstOrDefault(x => x.Id == id)
                                );
                            if (EnableCaching)
                                UpdateCache(result);
                        }
                    });
            }
            return result;
        }
    }

    public async Task<TModel> AddOrUpdateAsync(string id, TUpdate update)
    {
        if (string.IsNullOrWhiteSpace(id))
            return await AddAsync(update);
        return await UpdateAsync(id, update);
    }

    public async Task<TModel> AddAsync(TUpdate update)
    {
        using (benchmarkService.StartBenchmark())
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
                        if (EnableCaching && result != null)
                            UpdateCache(result);
                    });
                }
            });
            return result;
        }
    }

    public async Task<TModel> UpdateAsync(string id, TUpdate update = default)
    {
        using (benchmarkService.StartBenchmark())
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

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
                        if (EnableCaching && result != null)
                            UpdateCache(result);
                    });
                }
            });
            return result;
        }
    }

    public virtual async Task<List<TModel>> GetAllAsync()
    {
        await CacheAllIfNeededAsync();
        {
            if (EnableCaching && cacheHasAll)
                using (benchmarkService.StartBenchmark($"{typeof(TModel)}.GetAllAsync(Cache)"))
                    return cache.Values.ToList();

            List<TModel> results = null;
            using (benchmarkService.StartBenchmark($"{typeof(TModel)}.GetAllAsync(Realm)"))
                await Task.Run(() =>
                {
                    using (var realm = GetRealm())
                    {
                        results = realm.All<TRecord>()
                            .ToList()
                            .Select(x => ToModel(x))
                            .ToList();
                        if (results != null && EnableCaching)
                        {
                            LoadAllToCache(results);
                        }
                    }
                });

            return results;
        }
    }

    public async virtual Task<bool> DeleteAsync(string id)
    {
        using (benchmarkService.StartBenchmark())
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

                            if (EnableCaching)
                                MakeCacheDirty(id);
                        });
                    }
                }
            });

            return success;
        }
    }

    public async Task<int> CountAsync(Func<TInterface, bool> predicate = null)
    {
        await CacheAllIfNeededAsync();
        if (EnableCaching && cacheHasAll)
        {
            if (predicate == null) return cache.Count;
            return cache.Values.Count(predicate);
        }
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

    public async Task<long> LongCountAsync(Func<TInterface, bool> predicate = null)
    {
        await CacheAllIfNeededAsync();
        if (EnableCaching && cacheHasAll)
        {
            if (predicate == null) return cache.Count;
            return cache.Values.LongCount(predicate);
        }
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

    public async Task<List<TModel>> WhereAsync(Func<TInterface, bool> predicate)
    {
        return await WhereAsync(predicate, null);
    }

    public async Task<List<TModel>> WhereAsync(Func<TInterface, bool> predicate, Func<IEnumerable<TInterface>, IEnumerable<TInterface>> config)
    {
        await CacheAllIfNeededAsync();
        {
            List<TModel> results = null;
            await Task.Run(() =>
            {
                if (EnableCaching && cacheHasAll)
                {
                    using (benchmarkService.StartBenchmark($"{typeof(TModel)}.WhereAsync(Cache)"))
                    {
                        var all = cache.Values
                        .Cast<TInterface>()
                        .Where(predicate);
                        if (config != null)
                            all = config(all);
                        results = all
                            .Cast<TModel>()
                            .ToList();
                    }
                    return;
                }
                using (benchmarkService.StartBenchmark($"{typeof(TModel)}.WhereAsync(Realm)"))
                using (var realm = GetRealm())
                {
                    var all = realm.All<TRecord>().Where(predicate);
                    if (config != null) all = config(all);
                    results = all.ToList().Cast<TRecord>().Select(x => ToModel(x)).ToList();
                }
            });
            return results;
        }
    }

    public async Task<TModel> FirstOrDefaultAsync(Func<TInterface, bool> predicate)
    {
        return await FirstOrDefaultAsync(predicate, null);
    }

    public async Task<TModel> FirstOrDefaultAsync(Func<TInterface, bool> predicate, Func<IEnumerable<TInterface>, IEnumerable<TInterface>> config)
    {
        await CacheAllIfNeededAsync();
        TModel result = null;
        await Task.Run(() =>
        {
            if (EnableCaching && cacheHasAll)
            {
                using (benchmarkService.StartBenchmark($"{typeof(TModel)}.FirstOrDefaultAsync(Cache)"))
                {
                    var all = cache.Values
                    .Cast<TInterface>()
                    .Where(predicate);
                    if (config != null)
                        all = config(all);
                    result = all
                        .Cast<TModel>()
                        .FirstOrDefault();
                    return;
                }
            }
            using (benchmarkService.StartBenchmark($"{typeof(TModel)}.FirstOrDefaultAsync(Realm)"))
            using (var realm = GetRealm())
            {
                var all = realm.All<TRecord>().Where(predicate);
                if (config != null) all = config(all);
                result = ToModel(all.FirstOrDefault() as TRecord);
            }
        });
        return result;
    }

    public async Task CacheAllIfNeededAsync()
    {
        if (cacheHasAll) return;
        if (!EnableCaching) return;
        await ForceCacheAllAsync();
    }

    public async Task ForceCacheAllAsync()
    {
        if (AvoidConcurrentCacheAll && isCachingAll)
            return;
        await Task.Run(() =>
        {
            try
            {
                isCachingAll = true;
                using (benchmarkService.StartBenchmark($"{typeof(TModel)}.ForceCacheAllAsync()"))
                {
                    using (var realm = GetRealm())
                    {
                        var all = realm.All<TRecord>().ToList().Select(x => ToModel(x)).ToList();
                        LoadAllToCache(all);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                isCachingAll = false;
            }
        });
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}