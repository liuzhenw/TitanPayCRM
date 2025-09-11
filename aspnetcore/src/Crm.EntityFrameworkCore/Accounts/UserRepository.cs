using System;
using System.Linq;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Accounts;

public class UserRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, User, Guid, UserPagedParameter>(dbContextProvider),
    IUserRepository
{
    public Task<User?> FindByNameAsync(string name, bool includeDetails = true)
    {
        return FindAsync(x => x.Name == name, includeDetails);
    }

    public Task<User?> FindByEmailAsync(string email, bool includeDetails = true)
    {
        return FindAsync(x => x.Email == email, includeDetails);
    }

    public async Task<User?> FindLastAsync(bool includeInternalUser = false)
    {
        var queryable = await WithDetailsAsync(x => x.UserRoles);
        queryable = queryable.OrderByDescending(x => x.CreatedAt);
        return includeInternalUser
            ? await queryable.FirstOrDefaultAsync()
            : await queryable.FirstOrDefaultAsync(x => x.UserRoles.Count == 0);
    }
}