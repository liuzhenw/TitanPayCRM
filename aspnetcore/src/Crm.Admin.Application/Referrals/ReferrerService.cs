using Crm.Accounts;
using Crm.Admin.Permissions;
using Crm.Referrals;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Crm.Admin.Referrals;

[Authorize(CrmPermissions.Referrers.Default)]
public class ReferrerService(
    ReferralManager referralManager,
    IReferrerRepository referrerRepo,
    IReferralRelationRepository relationRepo,
    IUserRepository userRepo) : CrmAdminAppService, IReferrerService
{
    public async Task<ReferrerWithDetails> GetAsync(Guid id)
    {
        var referrer = await referrerRepo.GetAsync(id);
        return ObjectMapper.Map<Referrer, ReferrerWithDetails>(referrer);
    }

    public async Task<PagedResultDto<ReferrerDto>> GetPagedListAsync(ReferrerQueryInput input)
    {
        var parameter = new ReferrerPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await referrerRepo.GetPagedListAsync(parameter);
        return new PagedResultDto<ReferrerDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<Referrer>, List<ReferrerDto>>(paged.Items));
    }

    [Authorize(CrmPermissions.Referrers.Create)]
    public async Task<ReferrerWithDetails> CreateAsync(ReferrerCreateInput input)
    {
        var user = await userRepo.FindByEmailAsync(input.Email);
        if (user is null) throw new EntityNotFoundException(typeof(User));

        var referrer = await referralManager.CreateOrModifyReferrerAsync(user, input.LevelId);
        return ObjectMapper.Map<Referrer, ReferrerWithDetails>(referrer);
    }

    [Authorize(CrmPermissions.Referrers.Update)]
    public async Task<ReferrerWithDetails> UpdateAsync(Guid id, ReferrerUpdateInput input)
    {
        var referrer = await referrerRepo.GetAsync(id);
        if (input.LevelId != referrer.LevelId)
            await referralManager.ModifyReferrerLevelAsync(referrer, input.LevelId);
        referrer.IsDisabled = input.IsDisabled;
        referrer.Remark = input.Remark;
        await referrerRepo.UpdateAsync(referrer);
        return ObjectMapper.Map<Referrer, ReferrerWithDetails>(referrer);
    }
}