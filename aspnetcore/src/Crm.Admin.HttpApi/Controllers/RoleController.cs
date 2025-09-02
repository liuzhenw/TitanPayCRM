using Crm.Admin.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("accounts/roles")]
public class RoleController(IRoleService roleService) : CrmAdminController
{
    [HttpGet]
    public Task<List<RoleDto>> GetListAsync() => roleService.GetListAsync();
}