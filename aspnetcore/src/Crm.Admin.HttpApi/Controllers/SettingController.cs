using Crm.Admin.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("settings")]
public class SettingController(ISettingService settingService, IEmailTestService emailTestService) : CrmAdminController
{
    [HttpGet("{name}")]
    public Task<SettingItemDto> GetAsync(string name)
    {
        return settingService.GetAsync(name);
    }

    [HttpGet]
    public Task<Dictionary<string, List<SettingItemDto>>> GetAsync()
    {
        return settingService.GetListAsync();
    }

    [HttpPut]
    public Task UpdateAsync(SettingUpdateInput input)
    {
        return settingService.UpdateAsync(input);
    }
    
    [HttpPost("test/email")]
    public Task SendTestEmailAsync(TestEmailInput input)
    {
        return emailTestService.SendTestEmailAsync(input);
    }
}