using Volo.Abp.Domain.Entities;

namespace Astra.CacheStores;

public interface IEntityCacheStore<in TEntity, TEntityCache, in TKey>
    where TEntity : class, IEntity<TKey>
    where TEntityCache : class
{
    /// <summary>
    /// 当实体变更时触发
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task OnEntityChangedAsync(TEntity entity);
    Task<TEntityCache?> FindAsync(TKey id);
    Task<TEntityCache> GetAsync(TKey id);
    Task<List<TEntityCache>> GetListAsync();
    Task<TEntityCache> InsertAsync(TEntity entity);
    Task<TEntityCache> UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey entity);
}