using Crm.Accounts;
using Crm.Admin.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Crm.Admin.Accounts;

[Authorize(CrmPermissions.Roles.Default)]
public class RoleService(IRoleRepository repo) : CrmAdminAppService, IRoleService
{
    public async Task<List<RoleDto>> GetListAsync()
    {
        var roles = await repo.GetListAsync();
        return ObjectMapper.Map<List<Role>, List<RoleDto>>(roles);
    }
}