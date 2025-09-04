using System.Threading.Tasks;
using Crm.Users;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserService service) : CrmController
{
    [HttpGet]
    public Task<UserDto> GetAsync() => service.GetAsync();
    
    [HttpPut("password")]
    public Task<UserDto> ChangePasswordAsync(ChangePasswordInput input) =>service.ChangePasswordAsync(input);
}