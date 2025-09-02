using System;
using Volo.Abp.Domain.Entities;

namespace Crm.Accounts;

public class UserRole : Entity<Guid>
{
    protected UserRole() { }

    internal UserRole(Guid id, User user, Role role) : base(id)
    {
        UserId = user.Id;
        RoleId = role.Id;
    }

    public Guid UserId { get; private set; }
    public string RoleId { get; private set; } = null!;

    public virtual User User { get; private set; } = null!;
}