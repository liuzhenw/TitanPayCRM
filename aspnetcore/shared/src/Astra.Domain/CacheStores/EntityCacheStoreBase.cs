using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Astra.CacheStores;

public abstract class EntityCacheStoreBase<TEntity, TEntityCache, TKey>(
    IBasicRepository<TEntity, TKey> repository,
    IDistributedCache<TEntityCache, TKey> cacheEntity,
    IDistributedCache<EntityCacheList<TEntityCache>> cacheEntities,
    IDistributedCache<string, string> cacheItemKeys)
    : ITransientDependency
    where TEntity : class, IEntity<TKey>
    where TEntityCache : class
{
    protected readonly IBasicRepository<TEntity, TKey> Repository = repository;
    protected readonly IDistributedCache<TEntityCache, TKey> CacheEntity = cacheEntity;
    protected readonly IDistributedCache<EntityCacheList<TEntityCache>> CacheEntities = cacheEntities;
    protected readonly IDistributedCache<string, string> CacheItemKeys = cacheItemKeys;

    protected abstract TEntityCache MapToCache(TEntity entity);
    protected virtual TKey StringToCacheKey(string key)
    {
        if (typeof(TKey) == typeof(Guid))
            return (TKey)(object)Guid.Parse(key);

        return (TKey)Convert.ChangeType(key, typeof(TKey));
    }
    protected virtual string CacheKeyToString(TKey cacheKey)
    {
        if (cacheKey == null)
            throw new ArgumentNullException(nameof(cacheKey));

        return cacheKey.ToString()!;
    }
    protected virtual string CacheEntitiesCacheKey =>
        GetType().FullName ?? nameof(EntityCacheStoreBase<TEntity, TEntityCache, TKey>);

    protected virtual string BuildFindCacheKey(string key) => $"{CacheEntitiesCacheKey}:{key}";
    protected virtual Func<DistributedCacheEntryOptions>? CacheOptionsFactory => null;

    public virtual async Task OnEntityChangedAsync(TEntity entity)
    {
        await CacheEntity.RemoveAsync(entity.Id, true);
        await CacheEntities.RemoveAsync(CacheEntitiesCacheKey, true);
    }
    public virtual async Task<TEntityCache?> FindAsync(TKey id)
    {
        var entityCache = await CacheEntity.GetAsync(id);
        if (entityCache != null)
        {
            return entityCache;
        }

        var entity = await Repository.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entityCache = MapToCache(entity);
        await CacheEntity.SetAsync(id, entityCache);
        return entityCache;
    }

    public virtual Task<TEntityCache> GetAsync(TKey id)
    {
        return CacheEntity.GetOrAddAsync(
            id,
            async () => MapToCache(await Repository.GetAsync(id)),
            CacheOptionsFactory, false)!;
    }

    public virtual async Task<List<TEntityCache>> GetListAsync()
    {
        var entities = await CacheEntities.GetOrAddAsync(
            CacheEntitiesCacheKey,
            async () =>
            {
                var entities = await Repository.GetListAsync();
                return new EntityCacheList<TEntityCache>(entities.Select(MapToCache));
            },
            CacheOptionsFactory, false);
        return entities ?? [];
    }

    public virtual async Task<TEntityCache> InsertAsync(TEntity entity)
    {
        entity = await Repository.InsertAsync(entity);

        await OnEntityChangedAsync(entity);
        return MapToCache(entity);
    }

    public virtual async Task<TEntityCache> UpdateAsync(TEntity entity)
    {
        entity = await Repository.UpdateAsync(entity);

        await OnEntityChangedAsync(entity);
        return MapToCache(entity);
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await Repository.GetAsync(id);
        await Repository.DeleteAsync(entity);

        await OnEntityChangedAsync(entity);
    }

    public virtual async Task<TEntityCache?> FindAsync(
        string key,
        Func<Task<TEntity?>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null)
    {
        key = BuildFindCacheKey(key);
        TEntityCache? entityCache;
        var entityKey = await CacheItemKeys.GetAsync(key);
        if (entityKey != null)
        {
            entityCache = await FindAsync(StringToCacheKey(entityKey));
            if (entityCache != null)
            {
                return entityCache;
            }
        }

        var entity = await factory.Invoke();
        if (entity == null)
        {
            return null;
        }

        entityCache = MapToCache(entity);
        var options = optionsFactory?.Invoke() ?? CacheOptionsFactory?.Invoke();
        await CacheEntity.SetAsync(entity.Id, entityCache, options);
        await CacheItemKeys.SetAsync(key, CacheKeyToString(entity.Id), options);
        return entityCache;
    }
}