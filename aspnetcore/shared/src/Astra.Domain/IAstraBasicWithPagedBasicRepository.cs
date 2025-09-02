using Astra.Paged;
using Volo.Abp.Domain.Entities;

namespace Astra;

public interface IAstraBasicWithPagedBasicRepository<TEntity, TKey, in TPagedResult> :
    IAstraBasicRepository<TEntity, TKey>, IHasPagedRepository<TEntity, TPagedResult>
    where TEntity : Entity<TKey>
    where TPagedResult : PagedParameter<TEntity>;