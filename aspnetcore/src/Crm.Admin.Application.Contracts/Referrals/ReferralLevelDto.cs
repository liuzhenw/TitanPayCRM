using System.ComponentModel.DataAnnotations;
using FluentValidation;
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
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public uint Size { get; set; }
    public decimal Multiplier { get; set; }
    public string? Description { get; set; } 
}
public class ReferralLevelUpdateInput
{
    public string Name { get; set; } = null!;
    public uint Size { get; set; }
    public decimal Multiplier { get; set; }
    public string? Description { get; set; } 
}

public class ReferralLevelCreateInputValidator : AbstractValidator<ReferralLevelCreateInput>
{
    public ReferralLevelCreateInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Description).MaximumLength(255);
        RuleFor(x => x.Multiplier).GreaterThan(0);
    }
}

public class ReferralLevelUpdateInputValidator : AbstractValidator<ReferralLevelUpdateInput>
{
    public ReferralLevelUpdateInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Description).MaximumLength(255);
        RuleFor(x => x.Multiplier).GreaterThan(0);
    }
}