using Crm.Admin.Accounts;
using Crm.Admin.Products;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class CommissionLogDto : EntityDto<Guid>
{
    public ProductBasicDto Product { get; set; } = null!;
    public UserBasicDto Receiver { get; set; } = null!;
    public UserBasicDto Customer { get; set; } = null!;
    public ReferralLevelBasicDto Level { get; set; } = null!;
    public uint ReferralDepth { get; set; }
    public decimal Amount { get; set; }
    public Guid SaleLogId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class CommissionLogQueryInput : PagedAndSortedResultRequestDto
{
    public string? ProductId { get; set; }
    public Guid? ReceiverId { get; set; }
    public string? LevelId { get; set; }
    public Guid? CustomerId { get; set; }
    public ushort? ReferralDepth { get; set; }
}