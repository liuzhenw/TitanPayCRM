using System;
using System.Linq;
using Astra.Paged;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Crm.Referrals;

public class WithdrawalRequest : BasicAggregateRoot<Guid>, IHasConcurrencyStamp
{
    protected WithdrawalRequest() { }

    internal WithdrawalRequest(
        Guid guid, Referrer referrer, decimal amount, string toAddress, decimal fee) : base(guid)
    {
        if (fee < 0 || amount <= fee || toAddress.IsNullOrWhiteSpace())
            throw new UserFriendlyException("参数异常!");

        ReferrerId = referrer.Id;
        Amount = amount;
        Fee = fee;
        ToAddress = toAddress;
        Status = WithdrawalRequestStatus.Pending;
        CreatedAt = DateTimeOffset.Now;
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    public Guid ReferrerId { get; private set; }
    public WithdrawalRequestStatus Status { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Fee { get; private set; }
    public string ToAddress { get; private set; } = null!;
    public string? Txid { get; private set; }
    public string? RejectReason { get; private set; }
    public Guid? AuditorId { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string ConcurrencyStamp { get; set; } = null!;

    public void Approve(string txid, Guid auditorId)
    {
        EnsurePending();
        Txid = txid;
        Status = WithdrawalRequestStatus.Approved;
        AuditorId = auditorId;
        CompletedAt = DateTimeOffset.Now;
    }

    internal void Reject(string reason, Guid auditorId)
    {
        EnsurePending();
        RejectReason = reason;
        Status = WithdrawalRequestStatus.Rejected;
        AuditorId = auditorId;
        CompletedAt = DateTimeOffset.Now;
    }

    private void EnsurePending()
    {
        if (Status is not WithdrawalRequestStatus.Pending)
            throw new UserFriendlyException("此提款请求已处理!");
    }
}

public class WithdrawalRequestPagedParameter : PagedParameter<WithdrawalRequest>
{
    public Guid? ReferrerId { get; set; }
    public WithdrawalRequestStatus? Status { get; set; }
    public string? ToAddress { get; set; } = null!;
    public string? Txid { get; set; }

    public override IQueryable<WithdrawalRequest> BuildPagedQueryable(IQueryable<WithdrawalRequest> queryable)
    {
        return queryable
            .WhereIf(ReferrerId.HasValue, x => x.ReferrerId == ReferrerId)
            .WhereIf(Status.HasValue, x => x.Status == Status)
            .WhereIf(!ToAddress.IsNullOrWhiteSpace(), x => x.ToAddress == ToAddress)
            .WhereIf(!Txid.IsNullOrWhiteSpace(), x => x.Txid == Txid);
    }
}