using System;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Referrals;

public class WithdrawalRequestRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, WithdrawalRequest, Guid, WithdrawalRequestPagedParameter>(dbContextProvider),
    IWithdrawalRequestRepository
{
}