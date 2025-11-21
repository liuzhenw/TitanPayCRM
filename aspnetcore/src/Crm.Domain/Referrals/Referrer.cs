using System;
using System.Collections.Generic;
using System.Linq;
using Astra.Paged;
using Crm.Accounts;
using Crm.Events;
using Crm.Products;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Crm.Referrals;

public class Referrer : BasicAggregateRoot<Guid>, IHasConcurrencyStamp
{
    protected Referrer() { }

    internal Referrer(ReferrerRequest request) : base(request.Id)
    {
        SetLevelId(request.LevelId);
        CreatedAt = DateTimeOffset.Now;
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    internal Referrer(User user, ReferralLevel? level) : base(user.Id)
    {
        SetLevelId(level?.Id);
        CreatedAt = DateTimeOffset.Now;
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// 推荐等级
    /// </summary>
    public string? LevelId { get; private set; }

    /// <summary>
    /// 直推人数
    /// </summary>
    public uint DirectCount { get; private set; }

    /// <summary>
    /// 间推人数
    /// </summary>
    public uint IndirectCount { get; private set; }

    /// <summary>
    /// 总推荐人数
    /// </summary>
    public uint TotalCount { get; private set; }

    /// <summary>
    /// 累计佣金
    /// </summary>
    public decimal TotalCommission { get; private set; }

    /// <summary>
    /// 可用佣金
    /// </summary>
    public decimal Commission { get; private set; }

    /// <summary>
    /// 已提款数额
    /// </summary>
    public decimal Withdrawal { get; private set; }

    /// <summary>
    /// 提款地址
    /// </summary>
    public string? WithdrawalAddress { get; private set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisabled { get; set; }

    public string? Remark { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public List<SaleStatistic> Statistics { get; private set; } = [];
    public string ConcurrencyStamp { get; set; } = null!;

    public void SetWithdrawalAddress(string address)
    {
        WithdrawalAddress = address;
        UpdatedAt = DateTimeOffset.Now;
    }

    public void ChangeCommission(decimal commission)
    {
        Commission += commission;
        TotalCommission += commission;
        if (Commission < 0) Commission = 0;
        if (TotalCommission < 0) TotalCommission = 0;
        UpdatedAt = DateTimeOffset.Now;
    }

    internal void OnDirectReferralChanged(int count)
    {
        if (count == 0) return;

        var countAbs = (uint)Math.Abs(count);
        if (count < 0)
        {
            if (DirectCount < countAbs) DirectCount = 0;
            else DirectCount -= countAbs;
            if (TotalCount < countAbs) TotalCount = 0;
            else TotalCount -= countAbs;
        }
        else
        {
            DirectCount += countAbs;
            TotalCount += countAbs;
        }
        UpdatedAt = DateTimeOffset.Now;
    }

    internal void OnIndirectReferralChanged(int count)
    {
        if (count == 0) return;

        var countAbs = (uint)Math.Abs(count);
        if (count < 0)
        {
            if (IndirectCount < countAbs) IndirectCount = 0;
            else IndirectCount -= countAbs;
            if (TotalCount < countAbs) TotalCount = 0;
            else TotalCount -= countAbs;
        }
        else
        {
            IndirectCount += countAbs;
            TotalCount += countAbs;
        }

        UpdatedAt = DateTimeOffset.Now;
    }

    internal void OnCommissionAdded(ProductSaleLog log, decimal commission)
    {
        if (commission <= 0) return;

        TotalCommission += commission;
        Commission += commission;
        var statistic = Statistics.FirstOrDefault(x => x.ProductId == log.ProductId);
        if (statistic is null)
        {
            statistic = new SaleStatistic(log.ProductId);
            Statistics.Add(statistic);
        }

        statistic.AddSale(log, commission);
    }

    internal void OnWithdrawal(WithdrawalRequest request)
    {
        if (Commission < request.Amount)
            throw new BusinessException(CrmErrorCodes.Referrals.CommissionInsufficient);

        Commission -= request.Amount;
        Withdrawal += request.Amount;
        UpdatedAt = DateTimeOffset.Now;
    }

    internal void OnRefundWithdrawal(WithdrawalRequest request)
    {
        Commission += request.Amount;
        Withdrawal -= request.Amount;
        UpdatedAt = DateTimeOffset.Now;
    }

    internal void SetLevel(ReferralLevel? newLevel)
    {
        SetLevelId(newLevel?.Id);
        UpdatedAt = DateTimeOffset.Now;
    }

    private void SetLevelId(string? levelId)
    {
        LevelId = levelId;
        if (LevelId != null)
            AddLocalEvent(new ReferrerAddLevelDomainEvent(this));
    }
}

public class ReferrerPagedParameter : PagedParameter<Referrer>
{
    public Guid? Id { get; set; }
    public string? LevelId { get; set; }

    public override IQueryable<Referrer> BuildPagedQueryable(IQueryable<Referrer> queryable)
    {
        if (Id.HasValue) return queryable.Where(x => x.Id == Id);
        return queryable
            .WhereIf(!LevelId.IsNullOrWhiteSpace(), x => x.LevelId == LevelId);
    }
}