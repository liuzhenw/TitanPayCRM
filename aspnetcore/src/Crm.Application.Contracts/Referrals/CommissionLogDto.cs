using System;
using Crm.Products;
using Volo.Abp.Application.Dtos;

namespace Crm.Referrals;

public class CommissionLogDto : EntityDto<Guid>
{
    public ProductBasicDto Product { get; set; } = null!;
    public ReferralLevelBasicDto Level { get; set; } = null!;
    public string CustomerEmail { get; set; } = null!;
    public uint ReferralDepth { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class CommissionLogQueryInput : PagedAndSortedResultRequestDto
{
    public string? ProductId { get; set; }
    public string? LevelId { get; set; }
    public ushort? ReferralDepth { get; set; }
}