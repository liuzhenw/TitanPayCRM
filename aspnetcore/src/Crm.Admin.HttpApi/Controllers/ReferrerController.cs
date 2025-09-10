using Crm.Admin.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("referrals/referrers")]
public class ReferrerController(IReferrerService referrerService) : CrmAdminController
{
    [HttpGet("{id:guid}")]
    public Task<ReferrerWithDetails> GetAsync(Guid id) => referrerService.GetAsync(id);

    [HttpGet]
    public Task<PagedResultDto<ReferrerDto>> GetListAsync([FromQuery] ReferrerQueryInput input) =>
        referrerService.GetPagedListAsync(input);

    [HttpPost]
    public Task<ReferrerWithDetails> CreateAsync(ReferrerCreateInput input)
    {
        return referrerService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    public Task<ReferrerWithDetails> UpdateAsync(Guid id, [FromBody] ReferrerUpdateInput input) =>
        referrerService.UpdateAsync(id, input);

    [HttpGet("recommendees")]
    public Task<PagedResultDto<RecommendeeViewDto>> GetRecommendeePagedListAsync([FromQuery] RecommendeeViewQueryInput input) =>
        referrerService.GetRecommendeePagedListAsync(input);
}