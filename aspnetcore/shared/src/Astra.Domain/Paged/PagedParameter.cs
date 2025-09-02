using Volo.Abp.Domain.Entities;

namespace Astra.Paged;

public abstract class PagedParameter<TEntity>
    where TEntity : class, IEntity
{
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; } = 100;
    public string? Sorting { get; set; }

    public virtual IQueryable<TEntity> BuildPagedQueryable(IQueryable<TEntity> queryable)
    {
        return queryable;
    }
}