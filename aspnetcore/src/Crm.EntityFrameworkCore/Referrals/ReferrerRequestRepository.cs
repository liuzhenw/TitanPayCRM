using System;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Referrals;

public class ReferrerRequestRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, ReferrerRequest, Guid, ReferrerRequestPagedParameter>(dbContextProvider),
    IReferrerRequestRepository
{
}