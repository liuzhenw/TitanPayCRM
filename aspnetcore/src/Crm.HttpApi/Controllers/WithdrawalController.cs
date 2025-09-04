using System.Threading.Tasks;
using Crm.Referrals;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Controllers;

[ApiController]
[Route("withdrawal/requests")]
public class WithdrawalController(IWithdrawalRequestService service) : CrmController
{
    [HttpGet]
    public Task<PagedResultDto<WithdrawalRequestDto>> GetAsync([FromQuery] WithdrawalRequestQueryInput input) =>
        service.GetPagedListAsync(input);
    
    [HttpPost]
    public Task<WithdrawalRequestDto> CreateAsync(WithdrawalRequestCreateInput input) => service.CreateAsync(input);
}