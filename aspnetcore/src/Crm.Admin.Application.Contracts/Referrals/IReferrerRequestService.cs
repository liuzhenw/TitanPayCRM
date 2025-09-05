using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Referrals;

public interface IReferrerRequestService : IApplicationService
{
    Task<PagedResultDto<ReferrerRequestDto>> GetPagedListAsync(ReferrerRequestQueryInput input);
    Task<ReferrerRequestDto> ApproveAsync(Guid id);
    Task<ReferrerRequestDto> RejectAsync(Guid id, ReferrerRequestRejectInput input);
}