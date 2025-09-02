using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class ReferralRelationDto : EntityDto<Guid>
{
    public ReferralRelationUserDto Recommender { get; set; } = null!;
    public ReferralRelationUserDto Recommendee { get; set; } = null!;
    public uint Depth { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
public class ReferralRelationUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
}
public class ReferralRelationQueryInput : PagedAndSortedResultRequestDto
{
    public Guid? RecommenderId { get; set; }
    public Guid? RecommendeeId { get; set; }
    public uint? MinDepth { get; set; }
}