using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Astra.EntityFrameworkCore;

public abstract class AstraEfCoreRepository<TDbContext, TEntity, TKey>(
    IDbContextProvider<TDbContext> dbContextProvider) :
    EfCoreRepository<TDbContext, TEntity, TKey>(dbContextProvider)
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true)
    {
        var queryable = includeDetails ? await WithDetailsAsync() : await GetQueryableAsync();
        return await queryable.AnyAsync(predicate);
    }
    
    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true)
    {
        var queryable = includeDetails ? await WithDetailsAsync() : await GetQueryableAsync();
        return await queryable.CountAsync(predicate);
    }

    public async Task<List<TEntity>> GetListAsync(
        int limit, int skip = 0, string? sortBy = null, bool includeDetails = false, CancellationToken ct = default)
    {
        var queryable = includeDetails ? await WithDetailsAsync() : await GetQueryableAsync();
        queryable = sortBy is not null ? queryable.OrderBy(sortBy) : queryable.OrderByDescending(s => s.Id);
        return await queryable.Skip(skip).Take(limit).ToListAsync(ct);
    }
}