using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Referrals;

public interface ICommissionLogService : IApplicationService
{
    Task<PagedResultDto<CommissionLogDto>> GetPagedListAsync(CommissionLogQueryInput input);
}