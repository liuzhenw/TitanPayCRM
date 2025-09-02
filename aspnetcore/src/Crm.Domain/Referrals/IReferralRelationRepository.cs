using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astra;

namespace Crm.Referrals;

public interface IReferralRelationRepository: IAstraBasicRepository<ReferralRelation,Guid>
{
    Task<List<ReferralRelation>> GetAncestorListAsync(Guid recommendeeId, ushort? minDepth = null);
    Task<ReferralRelation?> FindParentAsync(Guid recommendeeId);
    Task<bool> ExistsAsync(Guid recommendeeId);
}