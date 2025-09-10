using Crm.Admin.Permissions;
using Crm.Referrals;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Referrals;

[Authorize(CrmPermissions.CommissionLogs.Default)]
public class CommissionLogService(ICommissionLogRepository repo) : CrmAdminAppService, ICommissionLogService
{
    public async Task<PagedResultDto<CommissionLogDto>> GetPagedListAsync(CommissionLogQueryInput input)
    {
        var parameter = new CommissionLogPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await repo.GetPagedListAsync(parameter);
        return new PagedResultDto<CommissionLogDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<CommissionLog>, List<CommissionLogDto>>(paged.Items));
    }
}