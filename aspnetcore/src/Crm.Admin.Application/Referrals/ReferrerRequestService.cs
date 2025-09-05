using Crm.Admin.Permissions;
using Crm.Referrals;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

[Authorize(CrmPermissions.ReferrerRequests.Default)]
public class ReferrerRequestService(ReferralManager manager, IReferrerRequestRepository repo) :
    CrmAdminAppService, IReferrerRequestService
{
    public async Task<PagedResultDto<ReferrerRequestDto>> GetPagedListAsync(ReferrerRequestQueryInput input)
    {
        var parameter = new ReferrerRequestPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await repo.GetPagedListAsync(parameter);
        return new PagedResultDto<ReferrerRequestDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<ReferrerRequest>, List<ReferrerRequestDto>>(paged.Items));
    }

    [Authorize(CrmPermissions.ReferrerRequests.Update)]
    public async Task<ReferrerRequestDto> ApproveAsync(Guid id)
    {
        var request = await repo.GetAsync(id);
        await manager.ApproveReferrerRequestAsync(request, CurrentUserId);
        return ObjectMapper.Map<ReferrerRequest, ReferrerRequestDto>(request);
    }

    [Authorize(CrmPermissions.ReferrerRequests.Update)]
    public async Task<ReferrerRequestDto> RejectAsync(Guid id, ReferrerRequestRejectInput input)
    {
        var request = await repo.GetAsync(id);
        if(input.IsDisabled) request.Disabled(input.Reason);
        else request.Reject(input.Reason);
        await repo.UpdateAsync(request);
        return ObjectMapper.Map<ReferrerRequest, ReferrerRequestDto>(request);
    }
}