using Astra.EntityFrameworkCore.DbMigrations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Crm.DbMigrations.Abp;

[ConnectionStringName("Default")]
public class AbpDbMigrationContext(DbContextOptions<AbpDbMigrationContext> options) : 
    AbpDbContext<AbpDbMigrationContext>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ConfigureSettingManagement();
        builder.ConfigureFeatureManagement();
        builder.ConfigurePermissionManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
    }
}

public class DefaultDbMigrationContextFactory : DesignTimeDbContextBase<AbpDbMigrationContext>
{
    public override AbpDbMigrationContext CreateDbContext(string[] args)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var optionsBuilder = new DbContextOptionsBuilder<AbpDbMigrationContext>()
            .UseNpgsql(
                GetConnectionStringFromPath(Directory.GetCurrentDirectory()),
                options => { options.MigrationsHistoryTable("__Migrations"); });
        return new AbpDbMigrationContext(optionsBuilder.Options);
    }
}