using Volo.Abp.Application.Services;

namespace Crm.Admin.Accounts;

public interface IRoleService : IApplicationService
{
    Task<List<RoleDto>> GetListAsync();
}