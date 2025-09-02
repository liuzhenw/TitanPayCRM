using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Referrals;

public class ReferralRelationRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraEfCoreRepository<CrmDbContext, ReferralRelation, Guid>(dbContextProvider),
    IReferralRelationRepository
{
    public async Task<List<ReferralRelation>> GetAncestorListAsync(Guid recommendeeId, ushort? minDepth = null)
    {
        var dbContext = await GetDbContextAsync();
        var query = dbContext.Set<ReferralRelation>()
            .Where(x => x.Recommendee.Id == recommendeeId);
        
        if (minDepth.HasValue)
        {
            query = query.Where(x => x.Depth >= minDepth.Value);
        }
        
        return await query.OrderBy(x => x.Depth).ToListAsync();
    }

    public async Task<ReferralRelation?> FindParentAsync(Guid recommendeeId)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.Set<ReferralRelation>()
            .FirstOrDefaultAsync(x => x.Recommendee.Id == recommendeeId && x.Depth == 1);
    }

    public async Task<bool> ExistsAsync(Guid recommendeeId)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.Set<ReferralRelation>()
            .AnyAsync(x => x.Recommendee.Id == recommendeeId);
    }
}