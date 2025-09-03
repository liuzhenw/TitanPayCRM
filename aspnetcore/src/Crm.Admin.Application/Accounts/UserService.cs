using Astra;
using Crm.Accounts;
using Crm.Admin.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Accounts;

[Authorize(CrmPermissions.Users.Default)]
public class UserService(UserManager manager, IUserRepository repo) : CrmAdminAppService, IUserService
{
    public async Task<UserWithDetailsDto> GetAsync(Guid id)
    {
        var user = await repo.GetAsync(id);
        return ObjectMapper.Map<User, UserWithDetailsDto>(user);
    }

    public async Task<PagedResultDto<UserDto>> GetPagedListAsync(UserQueryInput input)
    {
        var parameter = new UserPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await repo.GetPagedListAsync(parameter);
        return new PagedResultDto<UserDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<User>, List<UserDto>>(paged.Items));
    }

    [Authorize(CrmPermissions.Users.Update)]
    public async Task<UserWithDetailsDto> UpdateAsync(Guid id, UserUpdateInput input)
    {
        var user = await repo.GetAsync(id);
        if (user.Name == AstraConsts.RootUser)
            throw new UserFriendlyException("无法修改超级管理员的权限!");

        var roles = user.UserRoles.Select(x => x.RoleId).ToArray();
        var removeRoles = roles.Except(input.Roles).ToArray();
        if (removeRoles.Length > 0)
            await manager.RemoveRolesAsync(user, removeRoles);
        var appendRoles = input.Roles.Except(roles).ToArray();
        if (appendRoles.Length > 0)
            await manager.AddRolesAsync(user, appendRoles);
        await repo.UpdateAsync(user);
        return ObjectMapper.Map<User, UserWithDetailsDto>(user);
    }
}