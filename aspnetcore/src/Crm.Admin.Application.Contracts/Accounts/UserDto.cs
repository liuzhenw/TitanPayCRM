using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Accounts;

public class UserBasicDto : EntityDto<Guid>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AvatarUri { get; set; }
}
public class UserDto : UserBasicDto
{
    public decimal TotalConsumption { get; set; }
    public ushort Attempts { get; set; }
    public DateTimeOffset? LockedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class UserWithDetailsDto : UserDto
{
    public List<string> Roles { get; set; } = null!;
}

public class UserQueryInput : PagedAndSortedResultRequestDto
{
    public string? Email { get; set; }
    public string? Name { get; set; }
}

public class UserUpdateInput
{
    public List<string> Roles { get; set; } = null!;
}