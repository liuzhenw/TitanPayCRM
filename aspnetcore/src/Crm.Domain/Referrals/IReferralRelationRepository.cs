using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astra;
using Astra.Paged;

namespace Crm.Referrals;

public interface IReferralRelationRepository: IAstraBasicRepository<ReferralRelation,Guid>
{
    Task<List<ReferralRelation>> GetAncestorRelationListAsync(Guid recommendeeId, ushort? minDepth = null);
    Task<ReferralRelation?> FindParentAsync(Guid recommendeeId);
    Task<bool> ExistsAsync(Guid recommendeeId);
    Task<PagedList<RecommendeeQueryModel>> GetRecommendeePagedListAsync(RecommendeeQueryModelPagedParameter parameter);
    Task<List<AncestorQueryModel>> GetAncestorListAsync(Guid recommendeeId);
}