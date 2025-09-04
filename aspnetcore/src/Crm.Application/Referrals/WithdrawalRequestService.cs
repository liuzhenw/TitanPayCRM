using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Referrals;

[Authorize]
public class WithdrawalRequestService(
    ReferralManager manager, 
    IReferrerRepository referrerRepo,
    IWithdrawalRequestRepository requestRepo) :
    CrmAppService, IWithdrawalRequestService
{
    public async Task<WithdrawalRequestDto> CreateAsync(WithdrawalRequestCreateInput input)
    {
        var referrer = await referrerRepo.GetAsync(CurrentUserId);
        var request = await manager.CreateWithdrawalRequestAsync(referrer, input.Amount);
        return ObjectMapper.Map<WithdrawalRequest,WithdrawalRequestDto>(request);
    }

    public async Task<PagedResultDto<WithdrawalRequestDto>> GetPagedListAsync(WithdrawalRequestQueryInput input)
    {
        var parameter = new WithdrawalRequestPagedParameter { ReferrerId = CurrentUserId };
        ObjectMapper.Map(input, parameter);
        var paged = await requestRepo.GetPagedListAsync(parameter);
        return new PagedResultDto<WithdrawalRequestDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<WithdrawalRequest>, List<WithdrawalRequestDto>>(paged.Items));
    }
}