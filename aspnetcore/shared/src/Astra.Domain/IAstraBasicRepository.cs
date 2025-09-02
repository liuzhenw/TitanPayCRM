using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Astra;

public interface IAstraBasicRepository<TEntity, TKey> : IBasicRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    Task<List<TEntity>> GetListAsync(
        int limit, 
        int skip = 0, 
        string? sortBy = null,
        bool includeDetails = false, 
        CancellationToken ct = default);
}