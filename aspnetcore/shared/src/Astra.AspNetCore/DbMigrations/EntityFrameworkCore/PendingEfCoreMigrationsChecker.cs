using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Astra.DbMigrations.EntityFrameworkCore
{
    public abstract class PendingEfCoreMigrationsChecker<TDbContext>(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IAbpDistributedLock distributedLock,
        string databaseName) : PendingMigrationsCheckerBase
        where TDbContext : DbContext
    {
        protected IUnitOfWorkManager UnitOfWorkManager { get; } = unitOfWorkManager;
        protected IServiceProvider ServiceProvider { get; } = serviceProvider;
        protected ICurrentTenant CurrentTenant { get; } = currentTenant;
        protected IAbpDistributedLock DistributedLock { get; } = distributedLock;
        protected string DatabaseName { get; } = databaseName;

        protected virtual async Task LockAndApplyDatabaseMigrationsAsync()
        {
            await using (var lockHandle = await DistributedLock.TryAcquireAsync($"Migration_{DatabaseName}"))
            {
                if (lockHandle == null)
                {
                    Log.Information("{DatabaseName} 数据库迁移已被锁定!", DatabaseName);
                    return;
                }

                Log.Information("{DatabaseName} 数据库锁定迁移中...", DatabaseName);
                using (CurrentTenant.Change(null))
                {
                    using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                    try
                    {
                        var dbContext = ServiceProvider.GetRequiredService<TDbContext>();
                        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                        if (pendingMigrations.Any())
                        {
                            await dbContext.Database.MigrateAsync();
                        }

                        await uow.CompleteAsync();
                    }
                    catch (Exception e)
                    {
                        await uow.RollbackAsync();
                        Log.Error("{DatabaseName} 数据库迁移失败: {Error}", DatabaseName, e.Message);
                        throw;
                    }
                }
            }

            Log.Information("{DatabaseName} 数据库迁移已完成!", DatabaseName);
        }

        public virtual async Task CheckAndApplyDatabaseMigrationsAsync()
        {
            await TryAsync(LockAndApplyDatabaseMigrationsAsync);
        }
    }
}