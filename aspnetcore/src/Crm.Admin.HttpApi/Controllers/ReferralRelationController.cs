using Crm.Admin.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("referrals/relations")]
public class ReferralRelationController(IReferralRelationService service) : CrmAdminController
{
    [HttpGet]
    public Task<PagedResultDto<ReferralRelationDto>> GetAsync([FromQuery] ReferralRelationQueryInput input)
    {
        return service.GetPagedListAsync(input);
    }

    [HttpGet("{ancestorId:guid}/recommendees")]
    public Task<PagedResultDto<RecommendeeQueryModelDto>> GetRecommendeePagedListAsync(Guid ancestorId,
        [FromQuery] RecommendeeQueryModelQueryInput input) => service.GetRecommendeePagedListAsync(ancestorId, input);

    [HttpGet("{recommendeeId:guid}/ancestors")]
    public Task<List<AncestorQueryModelDto>> GetAncestorListAsync(Guid recommendeeId) =>
        service.GetAncestorListAsync(recommendeeId);
}