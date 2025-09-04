using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Crm.Users;

public interface IUserService:IApplicationService
{
    Task<UserDto> GetAsync();
    Task<UserDto> ChangePasswordAsync(ChangePasswordInput input);
}