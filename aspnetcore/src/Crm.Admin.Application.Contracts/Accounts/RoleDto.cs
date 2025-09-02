using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Accounts;

public class RoleBasicDto : EntityDto<string>
{
    public string Name { get; set; } = null!;
}

public class RoleDto : RoleBasicDto
{
    public bool IsStatic { get; set; }
    public bool IsPublic { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}