using System;
using System.Collections.Generic;
using System.Linq;
using Astra.Paged;

namespace Crm.Referrals;

public class RecommendeeQueryModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public uint ConsumptionCount { get; set; }
    public decimal TotalConsumption { get; set; }
    public string? LevelId { get; set; }
    public Guid RecommenderId { get; set; }
    public string RecommenderEmail { get; set; } = null!;
    public Guid AncestorId { get; set; }
    public string AncestorEmail { get; set; } = null!;
    public uint Depth { get; set; }
    public decimal TotalCommission { get; set; }
    public List<SaleStatistic>? Statistics { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class RecommendeeQueryModelPagedParameter : PagedParameter<RecommendeeQueryModel>
{
    public Guid? AncestorId { get; set; }
    public Guid? RecommenderId { get; set; }
    public string? LevelId { get; set; }
    public uint? Depth { get; set; }

    public override IQueryable<RecommendeeQueryModel> BuildPagedQueryable(IQueryable<RecommendeeQueryModel> queryable)
    {
        return queryable
            .WhereIf(AncestorId.HasValue, x => x.AncestorId == AncestorId)
            .WhereIf(RecommenderId.HasValue, x => x.RecommenderId == RecommenderId)
            .WhereIf(Depth.HasValue, x => x.Depth == Depth)
            .WhereIf(!LevelId.IsNullOrWhiteSpace(), x => x.LevelId == LevelId);
    }
}