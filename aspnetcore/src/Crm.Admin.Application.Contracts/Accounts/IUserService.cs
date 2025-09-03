using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Accounts;

public interface IUserService : IApplicationService
{
    Task<UserWithDetailsDto> GetAsync(Guid id);
    Task<PagedResultDto<UserDto>> GetPagedListAsync(UserQueryInput input);
    Task<UserWithDetailsDto> UpdateAsync(Guid id, UserUpdateInput input);
}