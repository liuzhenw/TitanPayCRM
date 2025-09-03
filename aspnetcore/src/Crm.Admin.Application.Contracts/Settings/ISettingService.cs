using Volo.Abp.Application.Services;

namespace Crm.Admin.Settings;

public interface ISettingService : IApplicationService
{
    Task<SettingItemDto> GetAsync(string name);
    Task<Dictionary<string, List<SettingItemDto>>> GetListAsync();
    Task UpdateAsync(SettingUpdateInput input);
}