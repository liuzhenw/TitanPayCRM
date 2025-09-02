using System;
using System.Threading.Tasks;
using Astra;

namespace Crm.Referrals;

public interface IReferrerRepository : IAstraBasicWithPagedBasicRepository<Referrer, Guid, ReferrerPagedParameter>
{
    Task<Referrer?> FindMaxLevelDescendantAsync(Guid recommenderId);
    Task<Referrer?> FindParentAsync(Guid recommendeeId);
}