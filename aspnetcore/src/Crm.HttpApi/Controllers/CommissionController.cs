using System.Threading.Tasks;
using Crm.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Controllers;

[ApiController]
[Route("commissions")]
public class CommissionController(ICommissionLogService service) : CrmController
{
    [HttpGet]
    public Task<PagedResultDto<CommissionLogDto>> GetAsync([FromQuery] CommissionLogQueryInput input) =>
        service.GetPagedListAsync(input);
}