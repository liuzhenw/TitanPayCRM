using Astra.DbMigrations.EntityFrameworkCore;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Crm.DbMigrations.Abp;

public class AbpDbMigrationChecker(
    IUnitOfWorkManager unitOfWorkManager,
    IServiceProvider serviceProvider,
    ICurrentTenant currentTenant,
    IAbpDistributedLock distributedLock) :
    PendingEfCoreMigrationsChecker<AbpDbMigrationContext>(
        unitOfWorkManager, serviceProvider, currentTenant, distributedLock, "crm_basic");