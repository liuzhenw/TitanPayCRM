using System.ComponentModel.DataAnnotations;
using Crm.Admin.Accounts;
using Crm.Referrals;
using FluentValidation;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class ReferrerRequestDto : EntityDto<Guid>
{
    public UserBasicDto User { get; set; } = null!;
    public ReferralLevelBasicDto Level { get; set; } = null!;
    public ReferrerRequestStatus Status { get; set; }
    public string? RejectReason { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class ReferrerRequestQueryInput : PagedAndSortedResultRequestDto
{
    public Guid? Id { get; set; }
    public string? LevelId { get; set; }
    public ReferrerRequestStatus? Status { get; set; }
}

public class ReferrerRequestRejectInput
{
    public string Reason { get; set; } = null!;
    public bool IsDisabled { get; set; }
}

public class ReferrerRequestRejectInputValidator : AbstractValidator<ReferrerRequestRejectInput>
{
    public ReferrerRequestRejectInputValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(255);
    }
}