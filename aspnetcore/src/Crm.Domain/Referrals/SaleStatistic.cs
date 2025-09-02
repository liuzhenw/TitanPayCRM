using System;
using Volo.Abp.Domain.Entities;

namespace Crm.Referrals;

public class SaleStatistic : Entity<Guid>
{
    protected SaleStatistic() { }

    internal SaleStatistic(Guid id, Guid referrerId, string productId) : base(id)
    {
        ReferrerId = referrerId;
        ProductId = productId;
    }

    public Guid ReferrerId { get; private set; }
    public string ProductId { get; private set; } = null!;

    /// <summary>
    /// 销售量
    /// </summary>
    public uint Volume { get; private set; }

    /// <summary>
    /// 销售额
    /// </summary>
    public decimal Revenue { get; private set; }

    /// <summary>
    /// 累计佣金
    /// </summary>
    public decimal Commission { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
    public virtual Referrer Referrer { get; private set; } = null!;

    public void AddSale(uint volume, decimal revenue, decimal commission)
    {
        Volume += volume;
        Revenue += revenue;
        Commission += commission;
        UpdatedAt = DateTimeOffset.Now;
    }
}