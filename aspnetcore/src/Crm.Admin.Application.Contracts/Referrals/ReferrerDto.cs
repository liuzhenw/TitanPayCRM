using Crm.Admin.Accounts;
using Crm.Admin.Products;
using FluentValidation;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class ReferrerDto : EntityDto<Guid>
{
    public UserBasicDto User { get; set; } = null!;
    public ReferralLevelBasicDto? Level { get; set; }
    public uint DirectCount { get; set; }
    public uint IndirectCount { get; set; }
    public uint TotalCount { get; set; }
    public decimal Commission { get; set; }
    public decimal TotalCommission { get; set; }
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
    public string? LevelId { get; set; } = null!;
    public bool IsDisabled { get; set; }
    public string? Remark { get; set; }
}

public class ReferrerChangeCommissionInput
{
    public decimal Commission { get; set; }
}

public class ReferrerCreateInput
{
    public string Email { get; set; } = null!;
    public string? LevelId { get; set; } = null!;
    public string? Remark { get; set; }
}

public class ReferrerUpdateInputValidator : AbstractValidator<ReferrerUpdateInput>
{
    public ReferrerUpdateInputValidator()
    {
        RuleFor(x => x.LevelId).MaximumLength(32);
        RuleFor(x => x.LevelId).MaximumLength(255);
    }
}

public class ReferrerCreateInputValidator : AbstractValidator<ReferrerCreateInput>
{
    public ReferrerCreateInputValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.LevelId).MaximumLength(32);
        RuleFor(x => x.LevelId).MaximumLength(255);
    }
}

public class ReferrerQueryInput : PagedAndSortedResultRequestDto
{
    public Guid? Id { get; set; }
    public string? LevelId { get; set; }
}

public class ReferrerSaleStatisticDto
{
    public ProductBasicDto Product { get; set; } = null!;
    public uint Volume { get; set; }
    public decimal Revenue { get; set; }
    public decimal Commission { get; set; }
}