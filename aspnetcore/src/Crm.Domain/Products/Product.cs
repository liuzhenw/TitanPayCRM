using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Crm.Products;

public class Product : BasicAggregateRoot<string>, ISoftDelete
{
    protected Product() { }

    public Product(string id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
        CreatedAt = DateTimeOffset.Now;
    }

    /// <summary>
    /// 商品名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 商品图片
    /// </summary>
    public string? ImageUri { get; set; }

    /// <summary>
    /// 商品简介
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 商品价格
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// 销售量
    /// </summary>
    public uint SalesVolume { get; private set; }

    /// <summary>
    /// 销售额
    /// </summary>
    /// <returns></returns>
    public decimal SalesRevenue { get; private set; }
    /// <summary>
    /// 累计佣金
    /// </summary>
    public decimal TotalCommission { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public void OnSold(ProductSaleLog log)
    {
        SalesRevenue += log.Amount;
        SalesVolume += log.Quantity;
        TotalCommission += log.TotalCommission;
        UpdatedAt = DateTimeOffset.Now;
    }
}

public class ProductCache
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ImageUri { get; set; }
}