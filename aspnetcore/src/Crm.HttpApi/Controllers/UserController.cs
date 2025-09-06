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
    
    [HttpGet("check/{email}")]
    public Task<UserCheckDto> CheckAsync(string email) => service.CheckAsync(email);
    
    [HttpPut("password")]
    public Task<UserDto> ChangePasswordAsync(ChangePasswordInput input) =>service.ChangePasswordAsync(input);
    
}