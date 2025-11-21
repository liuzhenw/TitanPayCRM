using Crm.Accounts;
using Crm.Admin.Permissions;
using Crm.Referrals;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

[Authorize(CrmPermissions.ReferralRelations.Default)]
public class ReferralRelationService(
    ReferralManager referralManager,
    IReferralRelationRepository relationRepo,
    IUserRepository userRepo) :
    CrmAdminAppService, IReferralRelationService
{
    [Authorize(CrmPermissions.ReferralRelations.Create)]
    public async Task CreateAsync(ReferralRelationCreateInput input)
    {
        var recommendee = await userRepo.GetAsync(input.RecommendeeId);
        var recommender = await userRepo.GetAsync(input.RecommenderId);
        await referralManager.CreateRelationAsync(recommender, recommendee);
    }
    
    public async Task<PagedResultDto<ReferralRelationDto>> GetPagedListAsync(ReferralRelationQueryInput input)
    {
        var parameter = new ReferralRelationPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await relationRepo.GetPagedListAsync(parameter);
        return new PagedResultDto<ReferralRelationDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<ReferralRelation>, List<ReferralRelationDto>>(paged.Items));
    }

    public async Task<PagedResultDto<RecommendeeQueryModelDto>> GetRecommendeePagedListAsync(
        Guid ancestorId, RecommendeeQueryModelQueryInput input)
    {
        var parameter = new RecommendeeQueryModelPagedParameter { AncestorId = ancestorId };
        ObjectMapper.Map(input, parameter);
        var paged = await relationRepo.GetRecommendeePagedListAsync(parameter);
        return new PagedResultDto<RecommendeeQueryModelDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<RecommendeeQueryModel>, List<RecommendeeQueryModelDto>>(paged.Items));
    }

    public async Task<List<AncestorQueryModelDto>> GetAncestorListAsync(Guid recommendeeId)
    {
        var ancestors = await relationRepo.GetAncestorListAsync(recommendeeId);
        return ObjectMapper.Map<List<AncestorQueryModel>, List<AncestorQueryModelDto>>(ancestors);
    }

    [Authorize(CrmPermissions.ReferralRelations.Delete)]
    public async Task RemoveAncestorRelationsAsync(Guid recommendeeId)
    {
        var user = await userRepo.GetAsync(recommendeeId);
        await referralManager.RemoveAncestorRelationAsync(user);
    }
    
}