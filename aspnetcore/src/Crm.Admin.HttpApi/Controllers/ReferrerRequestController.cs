using Crm.Admin.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("referrals/requests")]
public class ReferrerRequestController(IReferrerRequestService requestService) : CrmAdminController
{
    [HttpGet]
    public Task<PagedResultDto<ReferrerRequestDto>> GetListAsync([FromQuery] ReferrerRequestQueryInput input) =>
        requestService.GetPagedListAsync(input);

    [HttpPut("{id:guid}/approve")]
    public Task<ReferrerRequestDto> ApproveAsync(Guid id) => requestService.ApproveAsync(id);

    [HttpPut("{id:guid}/reject")]
    public Task<ReferrerRequestDto> RejectAsync(Guid id, [FromBody] ReferrerRequestRejectInput input) =>
        requestService.RejectAsync(id, input);
}