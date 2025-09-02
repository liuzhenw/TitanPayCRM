using System;
using Astra.Paged;
using Crm.Products;
using Volo.Abp.Domain.Entities;

namespace Crm.Referrals;

public class CommissionLog : BasicAggregateRoot<Guid>
{
    protected CommissionLog() { }

    public CommissionLog(
        Guid id, Referrer referrer, ReferralRelation relation, ProductSaleLog saleLog, decimal commission) : base(id)
    {
        ProductId = saleLog.ProductId;
        SaleLogId = saleLog.Id;
        ReceiverId = referrer.Id;
        ReferralLevelId = referrer.LevelId;
        DescendantId = relation.RecommendeeId;
        ReferralDepth = relation.Depth;
        Amount = commission;
        CreatedAt = DateTimeOffset.Now;
    }

    public string ProductId { get; private set; } = null!;
    public Guid SaleLogId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public string ReferralLevelId { get; private set; } = null!;
    public Guid DescendantId { get; private set; }
    public uint ReferralDepth { get; private set; }
    public decimal Amount { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
}

public class CommissionLogPagedParameter : PagedParameter<CommissionLog>
{
    public string? ProductId { get; set; }
    public Guid? ReferrerId { get; set; }
    public string? ReferrerLevelId { get; set; }
    public ushort? ReferralDepth { get; set; }
}