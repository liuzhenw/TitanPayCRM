using Volo.Abp.Domain.Entities;

namespace Astra.Paged;

public interface IHasPagedRepository<TEntity, in TPagedRequest>
    where TEntity : class, IEntity
    where TPagedRequest : PagedParameter<TEntity>
{
    Task<PagedList<TEntity>> GetPagedListAsync(TPagedRequest request);
}