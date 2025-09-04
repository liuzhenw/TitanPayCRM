using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Controllers;

[ApiController]
[Route("referral")]
public class ReferralController(IReferralService service) : CrmController
{
    [HttpGet("referrer")]
    public Task<ReferrerDto> GetReferrerAsync() => service.GetReferrerAsync();

    [HttpGet("levels")]
    public Task<List<ReferralLevelDto>> GetLevelsAsync() => service.GetLevelListAsync();

    [HttpGet("recommendees")]
    public Task<PagedResultDto<RecommendeeDto>> GetRecommendeesAsync([FromQuery] RecommendeeQueryInput input) =>
        service.GetRecommendeePagedListAsync(input);

    [HttpGet("request")]
    public Task<ReferrerRequestDto?> GetRequestAsync() => service.FindReferrerRequestAsync();

    [HttpPost("request")]
    public Task<ReferrerRequestDto> ApplyingAsync(ReferrerApplyInput input) => service.ReferrerApplyingAsync(input);

    [HttpPut("withdrawal-address")]
    public Task<ReferrerDto> UpdateWithdrawalAddressAsync(ReferrerWithdrawalAddressUpdateInput input) =>
        service.SetWithdrawalAddressAsync(input);
}