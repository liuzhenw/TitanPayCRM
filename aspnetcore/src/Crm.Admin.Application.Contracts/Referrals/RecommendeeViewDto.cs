using System;
using System.Collections.Generic;
using Crm.Admin.Accounts;
using Crm.Admin.Products;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class RecommendeeViewDto : EntityDto<Guid>
{
    public string Email { get; set; } = null!;
    public ReferralLevelBasicDto? Level { get; set; } = null!;
    public UserBasicDto Recommender { get; set; } = null!;
    public UserBasicDto Ancestor { get; set; } = null!;
    public uint Depth { get; set; }
    public decimal TotalCommission { get; set; }
    public List<ReferrerSaleStatisticDto>? Statistics { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}


public class RecommendeeViewQueryInput : PagedAndSortedResultRequestDto
{
    public Guid? AncestorId { get; set; }
    public Guid? RecommenderId { get; set; }
    public string? LevelId { get; set; }
    public uint? Depth { get; set; }
}