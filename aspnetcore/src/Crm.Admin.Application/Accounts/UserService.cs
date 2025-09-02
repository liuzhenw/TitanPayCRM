using Crm.Accounts;
using Crm.Admin.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Accounts;

[Authorize(CrmPermissions.Users.Default)]
public class UserService(IUserRepository repo) : CrmAdminAppService, IUserService
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
}