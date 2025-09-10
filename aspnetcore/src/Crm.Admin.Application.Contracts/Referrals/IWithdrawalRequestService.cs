using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Referrals;

public interface IWithdrawalRequestService : IApplicationService
{
    Task<PagedResultDto<WithdrawalRequestDto>> GetPagedListAsync(WithdrawalRequestQueryInput input);
    Task<WithdrawalRequestDto> GetAsync(Guid id);
    Task<WithdrawalRequestDto> ApproveAsync(Guid id, WithdrawalRequestApproveInput input);
    Task<WithdrawalRequestDto> RejectAsync(Guid id, WithdrawalRequestRejectInput input);
}