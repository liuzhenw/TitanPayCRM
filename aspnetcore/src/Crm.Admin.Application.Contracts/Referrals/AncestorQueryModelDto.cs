using Crm.Admin.Accounts;

namespace Crm.Admin.Referrals;

public class AncestorQueryModelDto
{
    public UserBasicDto User { get; set; } = null!;
    public uint Depth { get; set; }
    public ReferralLevelBasicDto? Level { get; set; }
    public decimal TotalCommission { get; set; }
    public List<ReferrerSaleStatisticDto>? Statistics { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}