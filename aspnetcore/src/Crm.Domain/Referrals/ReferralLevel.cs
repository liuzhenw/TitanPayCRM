using System;
using Crm.Accounts;
using Volo.Abp.Domain.Entities;

namespace Crm.Referrals;

public class ReferralLevel : BasicAggregateRoot<string>, IHasConcurrencyStamp
{
    protected ReferralLevel() { }

    internal ReferralLevel(string id, string name, uint size, decimal multiplier) : base(id)
    {
        Name = name;
        Size = size;
        Multiplier = multiplier;
        CreatedAt = DateTimeOffset.Now;
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// 等级名称
    /// </summary>
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 

    /// <summary>
    /// 等级大小
    /// </summary>
    public uint Size { get; private set; }

    /// <summary>
    /// 佣金系数
    /// </summary>
    public decimal Multiplier { get; set; }

    /// <summary>
    /// 该等级的人数
    /// </summary>
    public uint UserCount { get; private set; }

    /// <summary>
    /// 该等级累计佣金
    /// </summary>
    public decimal TotalCommission { get; private set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string ConcurrencyStamp { get; set; } = null!;
    
    internal void SetSize(uint size)
    {
        Size = size;
        UpdatedAt = DateTimeOffset.Now;
    }

    public void OnUserJoined()
    {
        UserCount++;
        UpdatedAt = DateTimeOffset.Now;
    }

    public void OnUserQuited()
    {
        UserCount--;
        UpdatedAt = DateTimeOffset.Now;
    }

    public void OnCommissionAdded(decimal commission)
    {
        TotalCommission += commission;
        UpdatedAt = DateTimeOffset.Now;
    }

    #region OverrideOperator

    public static bool operator ==(ReferralLevel? left, ReferralLevel? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Size == right.Size;
    }

    public static bool operator !=(ReferralLevel? left, ReferralLevel? right)
    {
        return !(left == right);
    }

    public static bool operator <(ReferralLevel left, ReferralLevel right)
    {
        return left.Size < right.Size;
    }

    public static bool operator >(ReferralLevel left, ReferralLevel right)
    {
        return left.Size > right.Size;
    }

    public static bool operator <=(ReferralLevel left, ReferralLevel right)
    {
        return left.Size <= right.Size;
    }

    public static bool operator >=(ReferralLevel left, ReferralLevel right)
    {
        return left.Size >= right.Size;
    }

    #endregion

    public override bool Equals(object? obj)
    {
        if (obj is ReferralLevel other)
            return this == other;

        return false;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Size.GetHashCode();
    }
}

public class ReferralLevelCache
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public uint Size { get; set; }
}