using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Referrals;

public interface ICommissionLogService : IApplicationService
{
    Task<PagedResultDto<CommissionLogDto>> GetPagedListAsync(CommissionLogQueryInput input);
}