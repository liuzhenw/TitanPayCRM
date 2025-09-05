using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Astra.Paged;

namespace Astra.EntityFrameworkCore;

public abstract class AstraPagedEfCoreRepository<TDbContext, TEntity, TKey, TPagedRequest>(
    IDbContextProvider<TDbContext> dbContextProvider) :
    AstraEfCoreRepository<TDbContext, TEntity, TKey>(dbContextProvider)
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
    where TPagedRequest : PagedParameter<TEntity>
{
    public virtual async Task<PagedList<TEntity>> GetPagedListAsync(TPagedRequest request)
    {
        // 过滤
        var queryable = request.BuildPagedQueryable(await GetQueryableAsync());
        
        // 排序
        queryable = string.IsNullOrWhiteSpace(request.Sorting)
            ? queryable.OrderByDescending(s => s.Id)
            : queryable.OrderBy(request.Sorting).ThenByDescending(s => s.Id);
        
        // 计数
        var count = await AsyncExecuter.CountAsync(queryable);
        
        // 分页
        queryable = queryable
            .Skip(request.SkipCount)
            .Take(request.MaxResultCount);
        var items = await AsyncExecuter.ToListAsync(queryable);

        return new PagedList<TEntity>(items, count);
    }
}