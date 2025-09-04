using System;
using Volo.Abp.Application.Dtos;

namespace Crm.Referrals;

public class WithdrawalRequestDto : EntityDto<Guid>
{
    public string Status { get; set; } = null!;
    public decimal Amount { get; set; }
    public string ToAddress { get; set; } = null!;
    public string? Txid { get; set; }
    public string? RejectReason { get; set; }
    public long CompletedAt { get; set; }
    public long CreatedAt { get; set; }
}

public class WithdrawalRequestQueryInput : PagedAndSortedResultRequestDto
{
    public WithdrawalRequestStatus? Status { get; set; }
    public string? ToAddress { get; set; } = null!;
    public string? Txid { get; set; }
}

public class WithdrawalRequestCreateInput
{
    public decimal Amount { get; set; }
}