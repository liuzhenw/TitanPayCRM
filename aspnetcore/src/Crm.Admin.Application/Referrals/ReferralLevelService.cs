using Crm.Admin.Permissions;
using Crm.Referrals;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;

namespace Crm.Admin.Referrals;

[Authorize(CrmPermissions.ReferralLevels.Default)]
public class ReferralLevelService(
    ReferralManager manager, IReferrerLevelRepository repo) : CrmAdminAppService, IReferralLevelService
{
    public async Task<List<ReferralLevelDto>> GetListAsync()
    {
        var levels = await repo.GetListAsync();
        return ObjectMapper.Map<List<ReferralLevel>, List<ReferralLevelDto>>(levels);
    }

    [Authorize(CrmPermissions.ReferralLevels.Create)]
    public async Task<ReferralLevelDto> CreateAsync(ReferralLevelCreateInput input)
    {
        var level = await manager.CreateReferralLevelAsync(input.Id, input.Name, input.Size, input.Multiplier);
        await repo.InsertAsync(level);
        return ObjectMapper.Map<ReferralLevel, ReferralLevelDto>(level);
    }

    [Authorize(CrmPermissions.ReferralLevels.Update)]
    public async Task<ReferralLevelDto> UpdateAsync(string id, ReferralLevelUpdateInput input)
    {
        var level = await repo.GetAsync(id);
        ObjectMapper.Map(input, level);
        if (level.Size != input.Size)
            await manager.ModifyReferralLevelSizeAsync(level, input.Size);
        await repo.UpdateAsync(level);
        return ObjectMapper.Map<ReferralLevel, ReferralLevelDto>(level);
    }

    [Authorize(CrmPermissions.ReferralLevels.Delete)]
    public async Task DeleteAsync(string id)
    {
        var level = await repo.GetAsync(id);
        if (level.UserCount > 0)
            throw new UserFriendlyException("此等级下存在用户,无法删除!");
        
        await repo.DeleteAsync(level);
    }
}