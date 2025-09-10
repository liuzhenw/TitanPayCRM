using System;
using System.Linq;
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
        LevelId = referrer.LevelId!;
        CustomerId = saleLog.CustomerId;
        CustomerEmail = saleLog.CustomerEmail;
        ReferralDepth = relation.Depth;
        Amount = commission;
        CreatedAt = DateTimeOffset.Now;
    }

    public string ProductId { get; private set; } = null!;
    public Guid SaleLogId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public string LevelId { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public string CustomerEmail { get; private set; } = null!;
    public uint ReferralDepth { get; private set; }
    public decimal Amount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}

public class CommissionLogPagedParameter : PagedParameter<CommissionLog>
{
    public string? ProductId { get; set; }
    public Guid? ReceiverId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? LevelId { get; set; }
    public ushort? ReferralDepth { get; set; }
    public Guid? SaleLogId { get; set; }

    public override IQueryable<CommissionLog> BuildPagedQueryable(IQueryable<CommissionLog> queryable)
    {
        return queryable
            .WhereIf(!ProductId.IsNullOrWhiteSpace(), x => x.ProductId == ProductId)
            .WhereIf(!LevelId.IsNullOrWhiteSpace(), x => x.LevelId == LevelId)
            .WhereIf(SaleLogId.HasValue, x => x.SaleLogId == SaleLogId)
            .WhereIf(ReceiverId.HasValue, x => x.ReceiverId == ReceiverId)
            .WhereIf(CustomerId.HasValue, x => x.CustomerId == CustomerId)
            .WhereIf(ReferralDepth.HasValue, x => x.ReferralDepth == ReferralDepth);
    }
}