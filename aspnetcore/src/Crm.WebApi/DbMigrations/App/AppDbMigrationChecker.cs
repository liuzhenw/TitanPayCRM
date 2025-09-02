using Astra.DbMigrations.EntityFrameworkCore;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Crm.DbMigrations.App;

public class AppDbMigrationChecker(
    IUnitOfWorkManager unitOfWorkManager,
    IServiceProvider serviceProvider,
    ICurrentTenant currentTenant,
    IAbpDistributedLock distributedLock) : PendingEfCoreMigrationsChecker<AppDbMigrationContext>(unitOfWorkManager,
    serviceProvider,
    currentTenant, distributedLock, "crm") { }