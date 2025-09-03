using Crm.Admin.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("account")]
public class AccountController(IAccountService service):CrmAdminController
{
    [HttpGet]
    public Task<UserWithDetailsDto> GetAsync() => service.GetAsync();
    
    [HttpPut("password")]
    public Task ChangePasswordAsync(ChangePasswordInput input) => service.ChangePasswordAsync(input);
}