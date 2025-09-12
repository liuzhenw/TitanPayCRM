using System;
using System.Collections.Generic;
using System.Linq;
using Astra.Paged;
using Crm.Products;
using Volo.Abp.Domain.Entities;

namespace Crm.Accounts;

public class User : BasicAggregateRoot<Guid>, IHasConcurrencyStamp
{
    protected User() { }

    internal User(Guid id, string name, string email) : this(id, name, email, DateTimeOffset.Now) { }

    internal User(Guid id, string name, string email, DateTimeOffset createdAt) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        CreatedAt = createdAt;
    }

    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public decimal TotalConsumption { get; private set; }
    public uint ConsumptionCount { get; private set; }
    public string? PasswordHash { get; internal set; }
    public string? AvatarUri { get; set; }
    public ushort Attempts { get; private set; }
    public DateTimeOffset? LockedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; internal set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string? PasswordSalt { get; internal set; }
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString("N");
    public virtual List<UserRole> UserRoles { get; internal set; } = [];

    internal void OnAttemptFailed()
    {
        Attempts++;
        if (Attempts >= 5)
            LockedAt = DateTimeOffset.Now.AddMinutes(5);
        UpdatedAt = DateTimeOffset.Now;
    }

    internal void OnAttemptSucceeded()
    {
        Attempts = 0;
        LockedAt = null;
        UpdatedAt = DateTimeOffset.Now;
    }

    internal void OnBuy(ProductSaleLog log)
    {
        ConsumptionCount++;
        TotalConsumption += log.Amount;
        UpdatedAt = DateTimeOffset.MaxValue;
    }
}

public class UserPagedParameter : PagedParameter<User>
{
    public string? Email { get; set; }
    public string? Name { get; set; }

    public override IQueryable<User> BuildPagedQueryable(IQueryable<User> queryable)
    {
        return queryable
            .WhereIf(!Name.IsNullOrWhiteSpace(), x => x.Name.Contains(Name!))
            .WhereIf(!Email.IsNullOrWhiteSpace(), x => x.Email.StartsWith(Email!));
    }
}

public class UserCache
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AvatarUri { get; set; }
}