using System;
using System.Linq;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Referrals;

public class ReferrerRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, Referrer, Guid, ReferrerPagedParameter>(dbContextProvider),
    IReferrerRepository
{
    public async Task<Referrer?> FindMaxLevelDescendantAsync(Guid recommenderId)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.ReferralRelations
            .Where(x => x.RecommenderId == recommenderId && x.Depth == 1)
            .Join(
                dbContext.Referrers,
                static referral => referral.RecommendeeId,
                static referrer => referrer.Id,
                static (referral, referrer) => referrer)
            .OrderByDescending(x => x.LevelId)
            .FirstOrDefaultAsync();
    }

    public async Task<Referrer?> FindParentAsync(Guid recommendeeId)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.ReferralRelations
            .Where(x => x.RecommendeeId == recommendeeId && x.Depth == 1)
            .Join(
                dbContext.Referrers,
                static referral => referral.RecommenderId,
                static referrer => referrer.Id,
                static (referral, referrer) => referrer)
            .FirstOrDefaultAsync();
    }
}