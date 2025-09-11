using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astra;

namespace Crm.Referrals;

public interface ICommissionLogRepository :
    IAstraBasicWithPagedBasicRepository<CommissionLog, Guid, CommissionLogPagedParameter>
{
    Task<List<CommissionLog>> GetZeroCommissionListAsync(Guid referrerId);
}