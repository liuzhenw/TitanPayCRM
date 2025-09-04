using Volo.Abp.Domain.Entities;

namespace Astra.Paged;

public class BasicPagedParameter
{
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; } = 100;
    public string? Sorting { get; set; }
}

public abstract class PagedParameter<TEntity> : BasicPagedParameter
    where TEntity : class, IEntity
{
    public virtual IQueryable<TEntity> BuildPagedQueryable(IQueryable<TEntity> queryable)
    {
        return queryable;
    }
}