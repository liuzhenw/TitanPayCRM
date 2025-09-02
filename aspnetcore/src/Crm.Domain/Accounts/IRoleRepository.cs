using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Crm.Accounts;

public interface IRoleRepository : IRepository<Role, string>
{
    Task<List<Role>> GetListByIdsAsync(params IEnumerable<string> ids);
}