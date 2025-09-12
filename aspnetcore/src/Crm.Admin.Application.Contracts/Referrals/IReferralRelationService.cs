using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Referrals;

public interface IReferralRelationService : IApplicationService
{
    Task<PagedResultDto<ReferralRelationDto>> GetPagedListAsync(ReferralRelationQueryInput input);
    Task<PagedResultDto<RecommendeeQueryModelDto>> GetRecommendeePagedListAsync(Guid ancestorId, RecommendeeQueryModelQueryInput input);
    Task<List<AncestorQueryModelDto>> GetAncestorListAsync(Guid recommendeeId);
}