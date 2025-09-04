using System;
using System.Linq;
using System.Text.Json.Nodes;
using Astra.Paged;
using Crm.Accounts;
using Volo.Abp.Domain.Entities;

namespace Crm.Products;

public class ProductSaleLog : BasicAggregateRoot<Guid>
{
    protected ProductSaleLog() { }

    internal ProductSaleLog(
        Guid id, Product product, User customer, string orderNo, uint quantity, JsonObject data) : base(id)
    {
        ProductId = product.Id;
        CustomerId = customer.Id;
        CustomerEmail = customer.Email;
        OrderNo = orderNo;
        Quantity = quantity;
        Amount = product.Price * quantity;
        Data = data;
        CreatedAt = DateTimeOffset.Now;
    }

    public string ProductId { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public string CustomerEmail { get; private set; } = null!;
    public string OrderNo { get; private set; } = null!;
    public uint Quantity { get; private set; }
    public decimal Amount { get; private set; }
    public JsonNode Data { get; private set; } = new JsonObject();
    public DateTimeOffset CreatedAt { get; private set; }
}

public class ProductSaleLogPagedParameter : PagedParameter<ProductSaleLog>
{
    public string? ProductId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? OrderNo { get; set; }

    public override IQueryable<ProductSaleLog> BuildPagedQueryable(IQueryable<ProductSaleLog> queryable)
    {
        return queryable
            .WhereIf(CustomerId.HasValue, x => x.CustomerId == CustomerId)
            .WhereIf(!ProductId.IsNullOrWhiteSpace(), x => x.ProductId == ProductId)
            .WhereIf(!OrderNo.IsNullOrWhiteSpace(), x => x.OrderNo == OrderNo);
    }
}