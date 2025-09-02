using System;
using System.Collections.Generic;
using System.Linq;
using Astra.Paged;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace Crm.Accounts;

public class User : BasicAggregateRoot<Guid>, IHasConcurrencyStamp
{
    protected User() { }

    internal User(Guid id, string email) : base(id)
    {
        if (email.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(email));
    }
    public string Email { get; private set; } = null!;
    public string? PasswordHash { get; internal set; }
    public string? AvatarUri { get; set; }
    public ushort Attempts { get; private set; }
    public DateTimeOffset? LockedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; internal set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
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
}


public class UserPagedParameter : PagedParameter<User>
{
    public string? Email { get; set; }

    public override IQueryable<User> BuildPagedQueryable(IQueryable<User> queryable)
    {
        return queryable
            .WhereIf(Email.IsNullOrWhiteSpace() is false, x => x.Email.StartsWith(Email!));
    }
}

public class UserCache
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? AvatarUri { get; set; }
}