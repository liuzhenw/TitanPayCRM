using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Referrals;

public interface IWithdrawalRequestService : IApplicationService
{
    Task<WithdrawalRequestDto> CreateAsync(WithdrawalRequestCreateInput input);
    Task<PagedResultDto<WithdrawalRequestDto>> GetPagedListAsync(WithdrawalRequestQueryInput input);
}