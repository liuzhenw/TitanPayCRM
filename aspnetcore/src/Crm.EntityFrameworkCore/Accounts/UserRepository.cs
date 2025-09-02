using System;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Accounts;

public class UserRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, User, Guid, UserPagedParameter>(dbContextProvider), IUserRepository
{
    public Task<User?> FindByNameAsync(string name, bool includeDetails = true)
    {
        return FindAsync(x => x.Name == name, includeDetails);
    }

    public Task<User?> FindByEmailAsync(string email, bool includeDetails = true)
    {
        return FindAsync(x => x.Email == email, includeDetails);
    }
}