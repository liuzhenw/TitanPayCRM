namespace Astra.Paged;

public class PagedList<TEntity>(List<TEntity> items, int totalCount)
    where TEntity : class
{
    public List<TEntity> Items { get; } = items;

    public int TotalCount { get; } = totalCount;
}