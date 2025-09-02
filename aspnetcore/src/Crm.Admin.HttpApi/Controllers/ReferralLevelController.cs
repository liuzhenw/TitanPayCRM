using Crm.Admin.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("referrals/levels")]
public class ReferralLevelController(IReferralLevelService levelService) : CrmAdminController
{
    [HttpGet]
    public Task<List<ReferralLevelDto>> GetListAsync() => levelService.GetListAsync();

    [HttpPost]
    public Task<ReferralLevelDto> CreateAsync([FromBody] ReferralLevelCreateInput input) => levelService.CreateAsync(input);

    [HttpPut("{id}")]
    public Task<ReferralLevelDto> UpdateAsync(string id, [FromBody] ReferralLevelUpdateInput input) => levelService.UpdateAsync(id, input);

    [HttpDelete("{id}")]
    public Task DeleteAsync(string id) => levelService.DeleteAsync(id);
}