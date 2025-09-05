using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Crm.Products;
using Volo.Abp.Domain.Values;

namespace Crm.Referrals;

public class SaleStatistic : ValueObject
{
    [JsonConstructor]
    protected SaleStatistic() { }

    public SaleStatistic(string productId)
    {
        ProductId = productId;
    }

    public string ProductId { get; set; } = null!;

    /// <summary>
    /// 销售量
    /// </summary>
    public uint Volume { get; set; }

    /// <summary>
    /// 销售额
    /// </summary>
    public decimal Revenue { get; set; }

    /// <summary>
    /// 累计佣金
    /// </summary>
    public decimal Commission { get; set; }

    public void AddSale(ProductSaleLog log, decimal commission)
    {
        Volume += log.Quantity;
        Revenue += log.Amount;
        Commission += commission;
    }

    public SaleStatistic Clone()
    {
        return new SaleStatistic
        {
            ProductId = ProductId,
            Commission = Commission,
            Revenue = Revenue,
            Volume = Volume
        };
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ProductId;
        yield return Volume;
        yield return Revenue;
        yield return Commission;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && ValueEquals(obj);
    }

    protected bool Equals(SaleStatistic other)
    {
        return ValueEquals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ProductId, Volume, Revenue, Commission);
    }
}