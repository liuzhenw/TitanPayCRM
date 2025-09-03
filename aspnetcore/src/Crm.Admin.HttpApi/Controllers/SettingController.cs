using Crm.Admin.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("settings")]
public class SettingController(ISettingService service) : CrmAdminController
{
    [HttpGet("{name}")]
    public Task<SettingItemDto> GetAsync(string name)
    {
        return service.GetAsync(name);
    }

    [HttpGet]
    public Task<Dictionary<string, List<SettingItemDto>>> GetAsync()
    {
        return service.GetListAsync();
    }

    [HttpPut]
    public Task UpdateAsync(SettingUpdateInput input)
    {
        return service.UpdateAsync(input);
    }
}