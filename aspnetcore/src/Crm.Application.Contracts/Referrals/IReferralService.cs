using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Referrals;

public interface IReferralService : IApplicationService
{
    Task<List<ReferralLevelDto>> GetLevelListAsync();
    Task<ReferrerDto> GetReferrerAsync();
    Task<ReferrerDto> SetWithdrawalAddressAsync(ReferrerWithdrawalAddressUpdateInput input);
    Task<PagedResultDto<RecommendeeDto>> GetRecommendeePagedListAsync(RecommendeeQueryInput input);
    Task<ReferrerRequestDto> ReferrerApplyingAsync(ReferrerApplyInput input);
    Task<ReferrerRequestDto?> FindReferrerRequestAsync();
    Task<List<AncestorQueryModelDto>> GetAncestorListAsync(string email);
}