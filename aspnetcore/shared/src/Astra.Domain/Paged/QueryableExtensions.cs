using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Entities;

namespace Astra.Paged;

public static class CollectionExtensions
{
    public static PagedList<TEntity> ToPagedResult<TEntity, TKey>(
        this IQueryable<TEntity> source, PagedParameter<TEntity> parameter, int totalCount)
        where TEntity : class, IEntity<TKey>
    {
        // 排序
        source = string.IsNullOrWhiteSpace(parameter.Sorting)
            ? source.OrderByDescending(s => s.Id)
            : source.OrderBy(parameter.Sorting).ThenByDescending(s => s.Id);
        // 分页
        var items = source
            .Skip(parameter.SkipCount)
            .Take(parameter.MaxResultCount)
            .ToList();
        return new PagedList<TEntity>(items, totalCount);
    }
}