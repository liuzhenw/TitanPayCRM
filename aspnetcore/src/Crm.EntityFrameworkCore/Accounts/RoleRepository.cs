using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Accounts;

public class RoleRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraEfCoreRepository<CrmDbContext, Role, string>(dbContextProvider), IRoleRepository
{
    public Task<List<Role>> GetListByIdsAsync(params IEnumerable<string> ids)
    {
        return GetListAsync(x => ids.Contains(x.Id));
    }
}