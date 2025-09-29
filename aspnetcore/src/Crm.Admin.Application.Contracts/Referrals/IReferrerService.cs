using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Referrals;

public interface IReferrerService : IApplicationService
{
    Task<ReferrerWithDetails> GetAsync(Guid id);
    Task<PagedResultDto<ReferrerDto>> GetPagedListAsync(ReferrerQueryInput input);
    Task<ReferrerWithDetails> CreateAsync(ReferrerCreateInput input);
    Task<ReferrerWithDetails> UpdateAsync(Guid id, ReferrerUpdateInput input);
    Task<ReferrerWithDetails> ChangeCommissionAsync(Guid id, ReferrerChangeCommissionInput input);
}