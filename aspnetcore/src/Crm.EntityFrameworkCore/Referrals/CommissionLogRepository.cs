using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Referrals;

public class CommissionLogRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, CommissionLog, Guid, CommissionLogPagedParameter>(dbContextProvider),
    ICommissionLogRepository
{
    public Task<List<CommissionLog>> GetZeroCommissionListAsync(Guid referrerId)
    {
        return GetListAsync(x => x.ReceiverId == referrerId && x.Amount == 0);
    }
}