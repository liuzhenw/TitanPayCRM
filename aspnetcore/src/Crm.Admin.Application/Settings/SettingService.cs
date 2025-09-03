using Crm.Admin.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Crm.Admin.Settings;

[Authorize(CrmPermissions.Settings.Default)]
public class SettingService(
    SettingDefinitionManager definitionManager,
    SettingManager manager) :
    CrmAdminAppService, ISettingService
{
    public async Task<SettingItemDto> GetAsync(string name)
    {
        var item = await definitionManager.GetAsync(name);
        return ObjectMapper.Map<SettingDefinition, SettingItemDto>(item);
    }

    public async Task<Dictionary<string, List<SettingItemDto>>> GetListAsync()
    {
        var definitions = await definitionManager.GetAllAsync();
        
        Dictionary<string, List<SettingItemDto>> results = [];
        foreach (var item in definitions)
        {
            var groupName = GetGroupName(item);
            if (!results.ContainsKey(groupName))
                results.Add(groupName, []);

            results[groupName].Add(ObjectMapper.Map<SettingDefinition, SettingItemDto>(item));
        }

        return results;
    }

    [Authorize(CrmPermissions.Settings.Update)]
    public async Task UpdateAsync(SettingUpdateInput input)
    {
        await manager.SetGlobalAsync(input.Name, input.Value);
    }

    private static string GetGroupName(SettingDefinition definition)
    {
        if (definition.Name.StartsWith("Abp.Mailing")) return "邮件";
        return definition.Name.Split('.').First();
    }
}