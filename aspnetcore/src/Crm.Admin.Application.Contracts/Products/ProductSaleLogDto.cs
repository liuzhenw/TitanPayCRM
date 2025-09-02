using Crm.Admin.Accounts;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Products;

public class ProductSaleLogDto : EntityDto<Guid>
{
    public ProductBasicDto Product { get; set; } = null!;
    public UserBasicDto Customer { get; set; } = null!;
    public string OrderNo { get; set; } = null!;
    public uint Quantity { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class ProductSaleLogQueryInput : PagedAndSortedResultRequestDto
{
    public string? ProductId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? OrderNo { get; set; }
}