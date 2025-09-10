using Crm.Admin.Permissions;
using Crm.Referrals;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

[Authorize(CrmPermissions.WithdrawalRequests.Default)]
public class WithdrawalRequestService(
    ReferralManager referralManager,
    IWithdrawalRequestRepository withdrawalRepo) :
    CrmAdminAppService, IWithdrawalRequestService
{
    public async Task<PagedResultDto<WithdrawalRequestDto>> GetPagedListAsync(WithdrawalRequestQueryInput input)
    {
        var parameter = new WithdrawalRequestPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await withdrawalRepo.GetPagedListAsync(parameter);
        return new PagedResultDto<WithdrawalRequestDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<WithdrawalRequest>, List<WithdrawalRequestDto>>(paged.Items));
    }

    public async Task<WithdrawalRequestDto> GetAsync(Guid id)
    {
        var request = await withdrawalRepo.GetAsync(id);
        return ObjectMapper.Map<WithdrawalRequest, WithdrawalRequestDto>(request);
    }

    [Authorize(CrmPermissions.WithdrawalRequests.Approve)]
    public async Task<WithdrawalRequestDto> ApproveAsync(Guid id, WithdrawalRequestApproveInput input)
    {
        var request = await withdrawalRepo.GetAsync(id);
        request.Approve(input.Txid, CurrentUserId);
        await withdrawalRepo.UpdateAsync(request);
        return ObjectMapper.Map<WithdrawalRequest, WithdrawalRequestDto>(request);
    }

    [Authorize(CrmPermissions.WithdrawalRequests.Reject)]
    public async Task<WithdrawalRequestDto> RejectAsync(Guid id, WithdrawalRequestRejectInput input)
    {
        var request = await withdrawalRepo.GetAsync(id);
        await referralManager.RefundWithdrawalAsync(request, input.Reason, CurrentUserId);
        return ObjectMapper.Map<WithdrawalRequest, WithdrawalRequestDto>(request);
    }
}