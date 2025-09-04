using Crm.Admin.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("referrals/relations")]
public class ReferralRelationController(IReferralRelationService service):CrmAdminController
{
    [HttpGet]
    public Task<PagedResultDto<ReferralRelationDto>> GetAsync([FromQuery] ReferralRelationQueryInput input)
    {
        return service.GetPagedListAsync(input);
    }
}