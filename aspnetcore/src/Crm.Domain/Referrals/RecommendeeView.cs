using System;
using System.Linq;
using Astra.Paged;

namespace Crm.Referrals;

public class RecommendeeView
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? LevelId { get; set; }
    public Guid RecommenderId { get; set; }
    public uint Depth { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class RecommendeeViewPagedParameter : PagedParameter<RecommendeeView>
{
    public Guid? RecommenderId { get; set; }
    public string? LevelId { get; set; }

    public override IQueryable<RecommendeeView> BuildPagedQueryable(IQueryable<RecommendeeView> queryable)
    {
        return queryable
            .WhereIf(RecommenderId.HasValue, x => x.RecommenderId == RecommenderId)
            .WhereIf(!LevelId.IsNullOrWhiteSpace(), x => x.LevelId == LevelId);
    }
}