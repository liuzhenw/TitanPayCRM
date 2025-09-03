using Crm.Accounts;
using Microsoft.AspNetCore.Authorization;

namespace Crm.Admin.Accounts;

[Authorize]
public class AccountService(UserManager manager, IUserRepository repo) :
    CrmAdminAppService, IAccountService
{
    public async Task<UserWithDetailsDto> GetAsync()
    {
        var me = await repo.GetAsync(CurrentUserId);
        return ObjectMapper.Map<User, UserWithDetailsDto>(me);
    }

    public async Task ChangePasswordAsync(ChangePasswordInput input)
    {
        var me = await repo.GetAsync(CurrentUserId);
        await manager.ChangePassword(me, input.OldPassword, input.NewPassword);
        await repo.UpdateAsync(me);
    }
}