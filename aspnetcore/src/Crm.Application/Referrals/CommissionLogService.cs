using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Referrals;

[Authorize]
public class CommissionLogService(ICommissionLogRepository repo) : CrmAppService, ICommissionLogService
{
    public async Task<PagedResultDto<CommissionLogDto>> GetPagedListAsync(CommissionLogQueryInput input)
    {
        var parameter = new CommissionLogPagedParameter { ReceiverId = CurrentUserId };
        ObjectMapper.Map(input, parameter);
        var paged = await repo.GetPagedListAsync(parameter);
        return new PagedResultDto<CommissionLogDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<CommissionLog>, List<CommissionLogDto>>(paged.Items));
    }
}