using System;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Referrals;

public class CommissionLogRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, CommissionLog, Guid, CommissionLogPagedParameter>(dbContextProvider),
    ICommissionLogRepository
{
}