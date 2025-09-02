using Crm.DbMigrations.Abp;
using Crm.DbMigrations.App;
using Serilog;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Crm.DbMigrations
{
    public class DbMigrationChecker(
        AbpDbMigrationChecker abpDbMigrationChecker,
        AppDbMigrationChecker appDbMigrationChecker,
        IDataSeeder dataSeeder) : 
        ITransientDependency
    {
        public async Task CheckAndApplyDatabaseMigrationsAsync()
        {
            Log.Information("==== 开始迁移数据库 ====");
            await abpDbMigrationChecker.CheckAndApplyDatabaseMigrationsAsync();
            await appDbMigrationChecker.CheckAndApplyDatabaseMigrationsAsync();
            Log.Information("==== 迁移数据库完成 ====");

            Log.Information("==== 开始初始化数据 ====");
            await dataSeeder.SeedAsync();
            Log.Information("==== 数据初始化完成 ====");
        }
    }
}