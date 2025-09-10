using Crm.Admin.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("referrals/commission-logs")]
public class CommissionLogsController(ICommissionLogService commissionLogService) : CrmAdminController
{
    [HttpGet]
    public Task<PagedResultDto<CommissionLogDto>> 
        GetListAsync([FromQuery] CommissionLogQueryInput input) => commissionLogService.GetPagedListAsync(input);
}