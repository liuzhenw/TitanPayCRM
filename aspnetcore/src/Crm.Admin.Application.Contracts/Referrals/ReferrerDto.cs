using Crm.Admin.Accounts;
using Crm.Admin.Products;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class ReferrerDto : EntityDto<Guid>
{
    public UserBasicDto User { get; set; } = null!;
    public ReferralLevelBasicDto Level { get; set; } = null!;
    public uint DirectCount { get; set; }
    public uint IndirectCount { get; set; }
    public uint TotalCount { get; set; }
    public decimal Commission { get; set; }
    public decimal Withdrawal { get; set; }
    public string? WithdrawalAddress { get; set; }
    public bool IsDisabled { get; set; }
    public string? Remark { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
public class ReferrerWithDetails : ReferrerDto
{
    public List<ReferrerSaleStatisticDto> Statistics { get; set; } = null!;
}
public class ReferrerUpdateInput
{
    public string LevelId { get; set; } = null!;
    public bool IsDisabled { get; set; }
    public string? Remark { get; set; }
}
public class ReferrerQueryInput : PagedAndSortedResultRequestDto
{
    public Guid? Id { get; set; }
    public string? LevelId { get; set; }
}
public class ReferrerSaleStatisticDto : EntityDto<Guid>
{
    public ProductBasicDto Product { get; set; } = null!;
    public uint Volume { get; set; }
    public decimal Revenue { get; set; }
    public decimal Commission { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}