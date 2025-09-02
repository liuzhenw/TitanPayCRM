using Volo.Abp.Application.Services;

namespace Crm.Admin.Referrals;

public interface IReferralLevelService : IApplicationService
{
    Task<List<ReferralLevelDto>> GetListAsync();
    Task<ReferralLevelDto> CreateAsync(ReferralLevelCreateInput input);
    Task<ReferralLevelDto> UpdateAsync(string id, ReferralLevelUpdateInput input);
    Task DeleteAsync(string id);
}