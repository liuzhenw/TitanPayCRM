using System;
using System.Linq;
using Astra.Paged;
using Crm.Accounts;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Crm.Referrals;

public class ReferrerRequest : BasicAggregateRoot<Guid>
{
    protected ReferrerRequest() { }

    internal ReferrerRequest(User user, ReferralLevel level) : base(user.Id)
    {
        LevelId = level.Id;
        Status = ReferrerRequestStatus.Pending;
        CreatedAt = DateTimeOffset.Now;
    }

    /// <summary>
    /// 推荐等级
    /// </summary>
    public string LevelId { get; private set; } = null!;

    public ReferrerRequestStatus Status { get; private set; }
    public string? RejectReason { get; private set; }

    public Guid? AuditorId { get; private set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; private set; }

    internal void Approve(Guid auditorId)
    {
        EnsurePending();
        Status = ReferrerRequestStatus.Approved;
        AuditorId = auditorId;
        UpdatedAt = DateTimeOffset.Now;
    }

    internal void Reset(ReferralLevel level)
    {
        if (Status is ReferrerRequestStatus.Disabled)
            throw new BusinessException(CrmErrorCodes.Referrals.RequestIsDisabled);

        if (Status is not ReferrerRequestStatus.Rejected)
            throw new BusinessException(CrmErrorCodes.Referrals.RequestRepeated);

        LevelId = level.Id;
        Status = ReferrerRequestStatus.Pending;
        UpdatedAt = DateTimeOffset.Now;
    }

    public void Reject(string reason)
    {
        EnsurePending();
        RejectReason = reason;
        Status = ReferrerRequestStatus.Rejected;
        UpdatedAt = DateTimeOffset.Now;
    }

    public void Disabled(string reason)
    {
        EnsurePending();
        RejectReason = reason;
        Status = ReferrerRequestStatus.Disabled;
        UpdatedAt = DateTimeOffset.Now;
    }

    private void EnsurePending()
    {
        if (Status is not ReferrerRequestStatus.Pending)
            throw new UserFriendlyException("此请求已处理!");
    }
}

public class ReferrerRequestPagedParameter : PagedParameter<ReferrerRequest>
{
    public Guid? Id { get; set; }
    public string? LevelId { get; set; }
    public ReferrerRequestStatus? Status { get; set; }

    public override IQueryable<ReferrerRequest> BuildPagedQueryable(IQueryable<ReferrerRequest> queryable)
    {
        return queryable
            .WhereIf(Id.HasValue, x => x.Id == Id)
            .WhereIf(!LevelId.IsNullOrWhiteSpace(), x => x.LevelId == LevelId)
            .WhereIf(Status.HasValue, x => x.Status == Status);
    }
}