using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Accounts;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Referrals;

[Authorize]
public class ReferralService(
    ReferralManager manager,
    IUserRepository userRepo,
    IReferrerLevelRepository levelRepo,
    IReferrerRepository referrerRepo,
    IReferralRelationRepository relationRepo,
    IReferrerRequestRepository requestRepo) :
    CrmAppService, IReferralService
{
    [AllowAnonymous]
    public async Task<List<ReferralLevelDto>> GetLevelListAsync()
    {
        var levels = await levelRepo.GetListAsync();
        return ObjectMapper.Map<List<ReferralLevel>, List<ReferralLevelDto>>(levels);
    }
    public async Task<ReferrerDto> GetReferrerAsync()
    {
        var referrer = await referrerRepo.FindAsync(CurrentUserId);
        if (referrer == null) return new ReferrerDto { Id = CurrentUserId };
        return ObjectMapper.Map<Referrer, ReferrerDto>(referrer);
    }

    public async Task<ReferrerDto> SetWithdrawalAddressAsync(ReferrerWithdrawalAddressUpdateInput input)
    {
        var referrer = await referrerRepo.GetAsync(CurrentUserId);
        referrer.SetWithdrawalAddress(input.Address);
        await referrerRepo.UpdateAsync(referrer);
        return ObjectMapper.Map<Referrer, ReferrerDto>(referrer);
    }

    public async Task<PagedResultDto<RecommendeeDto>> GetRecommendeePagedListAsync(RecommendeeQueryInput input)
    {
        var parameter = new RecommendeeQueryModelPagedParameter { AncestorId = CurrentUserId };
        ObjectMapper.Map(input, parameter);
        var paged = await relationRepo.GetRecommendeePagedListAsync(parameter);
        return new PagedResultDto<RecommendeeDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<RecommendeeQueryModel>, List<RecommendeeDto>>(paged.Items));
    }

    public async Task<ReferrerRequestDto> ReferrerApplyingAsync(ReferrerApplyInput input)
    {
        var user = await userRepo.GetAsync(CurrentUserId);
        var request = await manager.ReferrerApplyingAsync(user, input.LevelId);
        return ObjectMapper.Map<ReferrerRequest, ReferrerRequestDto>(request);
    }
    
    public async Task<ReferrerRequestDto?> FindReferrerRequestAsync()
    {
        var request = await requestRepo.FindAsync(CurrentUserId);
        if (request == null) return null;
        return ObjectMapper.Map<ReferrerRequest, ReferrerRequestDto>(request);
    }
}