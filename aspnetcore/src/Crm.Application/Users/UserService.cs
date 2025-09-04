using System;
using System.Threading.Tasks;
using Crm.Accounts;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;

namespace Crm.Users;

[Authorize]
public class UserService(IUserRepository repo, UserManager userManager) : CrmAppService, IUserService
{
    public async Task<UserDto> GetAsync()
    {
        var user = await repo.GetAsync(CurrentUserId);
        return ObjectMapper.Map<User, UserDto>(user);
    }

    public async Task<UserDto> ChangePasswordAsync(ChangePasswordInput input)
    {
        var user = await repo.GetAsync(CurrentUserId);
        if (input.OldPassword.IsNullOrWhiteSpace() && !user.PasswordHash.IsNullOrWhiteSpace())
            throw new BusinessException(CrmErrorCodes.Accounts.InvalidPassword);

        await userManager.ChangePassword(user, input.OldPassword, input.NewPassword);
        await repo.UpdateAsync(user);
        return ObjectMapper.Map<User, UserDto>(user);
    }
}