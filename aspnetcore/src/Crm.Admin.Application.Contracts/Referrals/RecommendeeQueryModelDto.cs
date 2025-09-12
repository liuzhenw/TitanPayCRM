using Crm.Admin.Accounts;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class RecommendeeQueryModelDto : EntityDto<Guid>
{
    public string Email { get; set; } = null!;
    public ReferralLevelBasicDto? Level { get; set; }
    public UserBasicDto Recommender { get; set; } = null!;
    public UserBasicDto Ancestor { get; set; } = null!;
    public uint Depth { get; set; }
    public decimal TotalCommission { get; set; }
    public List<ReferrerSaleStatisticDto>? Statistics { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}


public class RecommendeeQueryModelQueryInput : PagedAndSortedResultRequestDto
{
    public Guid? RecommenderId { get; set; }
    public string? LevelId { get; set; }
    public uint? Depth { get; set; }
}