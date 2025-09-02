using Crm.Admin.Accounts;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("accounts/users")]
public class UserController(IUserService userService) : CrmAdminController
{
    [HttpGet("{id:guid}")]
    public Task<UserWithDetailsDto> GetAsync(Guid id) => userService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<UserDto>> GetListAsync([FromQuery] UserQueryInput input) => userService.GetPagedListAsync(input);
}