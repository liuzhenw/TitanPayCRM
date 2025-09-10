using Crm.Admin.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("referrals/withdrawal-requests")]
public class WithdrawalRequestController(IWithdrawalRequestService withdrawalRequestService) : CrmAdminController
{
    [HttpGet]
    public Task<PagedResultDto<WithdrawalRequestDto>> GetListAsync([FromQuery] WithdrawalRequestQueryInput input) =>
        withdrawalRequestService.GetPagedListAsync(input);

    [HttpGet("{id:guid}")]
    public Task<WithdrawalRequestDto> GetAsync(Guid id) => withdrawalRequestService.GetAsync(id);

    [HttpPut("{id:guid}/approve")]
    public Task<WithdrawalRequestDto> ApproveAsync(Guid id, [FromBody] WithdrawalRequestApproveInput input) =>
        withdrawalRequestService.ApproveAsync(id, input);

    [HttpPut("{id:guid}/reject")]
    public Task<WithdrawalRequestDto> RejectAsync(Guid id, [FromBody] WithdrawalRequestRejectInput input) =>
        withdrawalRequestService.RejectAsync(id, input);
}