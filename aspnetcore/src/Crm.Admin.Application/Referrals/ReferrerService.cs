using Crm.Admin.Permissions;
using Crm.Referrals;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

[Authorize(CrmPermissions.Referrers.Default)]
public class ReferrerService(ReferralManager referralManager, IReferrerRepository repo) : CrmAdminAppService, IReferrerService
{
    public async Task<ReferrerWithDetails> GetAsync(Guid id)
    {
        var referrer = await repo.GetAsync(id);
        return ObjectMapper.Map<Referrer, ReferrerWithDetails>(referrer);
    }

    public async Task<PagedResultDto<ReferrerDto>> GetPagedListAsync(ReferrerQueryInput input)
    {
        var parameter = new ReferrerPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await repo.GetPagedListAsync(parameter);
        return new PagedResultDto<ReferrerDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<Referrer>, List<ReferrerDto>>(paged.Items));
    }

    [Authorize(CrmPermissions.Referrers.Update)]
    public async Task<ReferrerWithDetails> UpdateAsync(Guid id, ReferrerUpdateInput input)
    {
        var referrer = await repo.GetAsync(id);
        if (input.LevelId != referrer.LevelId)
            await referralManager.ModifyReferrerLevelAsync(referrer, input.LevelId);
        referrer.IsDisabled = input.IsDisabled;
        referrer.Remark = input.Remark;
        await repo.UpdateAsync(referrer);
        return ObjectMapper.Map<Referrer, ReferrerWithDetails>(referrer);
    }
}