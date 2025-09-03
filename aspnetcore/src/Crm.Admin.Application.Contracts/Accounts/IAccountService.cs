using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Accounts;

public interface IAccountService : IApplicationService
{
    Task<UserWithDetailsDto> GetAsync();
    Task ChangePasswordAsync(ChangePasswordInput input);
}

public class ChangePasswordInput
{
    [Required]
    public string OldPassword { get; set; } = null!;
    [Required]
    public string NewPassword { get; set; } = null!;
}