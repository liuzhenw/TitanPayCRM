namespace Astra.CacheStores;

public class EntityCacheList<TEntity> : List<TEntity>
{
    public EntityCacheList()
    {
    }

    public EntityCacheList(IEnumerable<TEntity> collection) : base(collection)
    {
    }
}