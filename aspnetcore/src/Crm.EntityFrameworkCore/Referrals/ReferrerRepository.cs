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
    public async Task<Referrer?> FindMaxLevelDescendantAsync(Guid ancestorId)
    {
        var dbContext = await GetDbContextAsync();
        var queryable = from rr in dbContext.ReferralRelations
            where rr.Ancestor.Id == ancestorId
            join er in dbContext.Referrers on rr.Recommendee.Id equals er.Id
            join le in dbContext.ReferralLevels on er.LevelId equals le.Id
            orderby le.Size descending
            select er;
        return await queryable.FirstOrDefaultAsync();
    }

    public async Task<Referrer?> FindParentAsync(Guid recommendeeId)
    {
        var dbContext = await GetDbContextAsync();
        
        var queryable = from rr in dbContext.ReferralRelations
            where rr.Recommendee.Id == recommendeeId && rr.Depth == 1
            join er in dbContext.Referrers on rr.Recommender.Id equals er.Id
            select er;
        return await queryable.FirstOrDefaultAsync();
    }
}