using Crm.Admin.Accounts;
using Crm.Referrals;
using FluentValidation;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

public class WithdrawalRequestDto : EntityDto<Guid>
{
    public UserBasicDto Referrer { get; set; } = null!;
    public WithdrawalRequestStatus Status { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public string ToAddress { get; set; } = null!;
    public string? Txid { get; set; }
    public string? RejectReason { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class WithdrawalRequestQueryInput : PagedAndSortedResultRequestDto
{
    public Guid? ReferrerId { get; set; }
    public WithdrawalRequestStatus? Status { get; set; }
    public string? ToAddress { get; set; }
    public string? Txid { get; set; }
}

public class WithdrawalRequestApproveInput
{
    public string Txid { get; set; } = null!;
}

public class WithdrawalRequestRejectInput
{
    public string Reason { get; set; } = null!;
}

public class WithdrawalRequestApproveInputValidator : AbstractValidator<WithdrawalRequestApproveInput>
{
    public WithdrawalRequestApproveInputValidator()
    {
        RuleFor(x => x.Txid).NotEmpty().MaximumLength(128);
    }
}

public class WithdrawalRequestRejectInputValidator : AbstractValidator<WithdrawalRequestRejectInput>
{
    public WithdrawalRequestRejectInputValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(255);
    }
}