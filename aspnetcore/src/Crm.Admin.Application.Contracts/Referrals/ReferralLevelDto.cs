using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class ReferralLevelBasicDto : EntityDto<string>
{
    public string Name { get; set; } = null!;
    public uint Size { get; set; }
}
public class ReferralLevelDto : ReferralLevelBasicDto
{
    public string? Description { get; set; } 
    public decimal Multiplier { get; set; }
    public uint UserCount { get; set; }
    public decimal TotalCommission { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
public class ReferralLevelCreateInput
{
    [Required]
    [MaxLength(32)]
    public string Id { get; set; } = null!;

    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    public uint Size { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Multiplier { get; set; }
    public string? Description { get; set; } 
}
public class ReferralLevelUpdateInput
{
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    public uint Size { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Multiplier { get; set; }
    public string? Description { get; set; } 
}