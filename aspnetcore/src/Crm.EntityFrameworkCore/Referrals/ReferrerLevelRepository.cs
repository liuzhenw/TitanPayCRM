using System.Linq;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Referrals;

public class ReferrerLevelRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraEfCoreRepository<CrmDbContext, ReferralLevel, string>(dbContextProvider),
    IReferrerLevelRepository
{
    public async Task<bool> ExistsAsync(uint size)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.Set<ReferralLevel>()
            .AnyAsync(x => x.Size == size);
    }

    public override async Task<IQueryable<ReferralLevel>> GetQueryableAsync()
    {
        var queryable = await base.GetQueryableAsync();
        return queryable.OrderBy(x => x.Size);
    }
}