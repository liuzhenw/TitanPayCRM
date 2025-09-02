using System;
using Volo.Abp.Domain.Entities;

namespace Crm.Accounts;

public class Role : BasicAggregateRoot<string>
{
    protected Role() { }

    public Role(string id, string name, bool isStatic, bool isPublic) : base(id)
    {
        Name = name;
        IsStatic = isStatic;
        IsPublic = isPublic;
    }

    public string Name { get; private set; } = null!;
    public bool IsStatic { get; private set; }
    public bool IsPublic { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
}

public class RoleCache
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}