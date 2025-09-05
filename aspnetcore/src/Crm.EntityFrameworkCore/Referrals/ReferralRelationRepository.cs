using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Astra.Paged;
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
            query = query.Where(x => x.Depth >= minDepth.Value);

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

    public async Task<PagedList<RecommendeeView>> GetRecommendeePagedListAsync(RecommendeeViewPagedParameter parameter)
    {
        var dbContext = await GetDbContextAsync();
        // 连表(LeftJoin)
        var joinQueryable = from rr in dbContext.ReferralRelations
            join er in dbContext.Referrers on rr.Recommendee.Id equals er.Id into g
            from er in g.DefaultIfEmpty()
            select new RecommendeeView
            {
                Id = rr.Recommendee.Id,
                RecommenderId = rr.Recommender.Id,
                Email = rr.Recommendee.Email,
                Depth = rr.Depth,
                CreatedAt = rr.CreatedAt,
                LevelId = er == null ? null : er.LevelId
            };
        
        // 过滤
        joinQueryable = parameter.BuildPagedQueryable(joinQueryable);
        
        // 排序
        joinQueryable = string.IsNullOrWhiteSpace(parameter.Sorting)
            ? joinQueryable.OrderByDescending(s => s.Id)
            : joinQueryable.OrderBy(parameter.Sorting).ThenByDescending(s => s.Id);
        
        // 计数
        var totalCount = await joinQueryable.CountAsync();

        // 分页
        joinQueryable = joinQueryable
            .Skip(parameter.SkipCount)
            .Take(parameter.MaxResultCount);
        var items = await AsyncExecuter.ToListAsync(joinQueryable);
        return new PagedList<RecommendeeView>(items, totalCount);
    }
}