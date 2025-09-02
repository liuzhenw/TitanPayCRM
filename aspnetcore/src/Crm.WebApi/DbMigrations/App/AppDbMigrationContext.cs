using Astra.EntityFrameworkCore.DbMigrations;
using Crm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;

namespace Crm.DbMigrations.App;

[ConnectionStringName(CrmDbProperties.ConnectionStringName)]
public class AppDbMigrationContext(DbContextOptions<CrmDbContext> options) : CrmDbContext(options);

public class AppMigrationDbContextFactory : DesignTimeDbContextBase<AppDbMigrationContext>
{
    public override AppDbMigrationContext CreateDbContext(string[] args)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        var optionsBuilder = new DbContextOptionsBuilder<CrmDbContext>()
            .UseNpgsql(
                GetConnectionStringFromPath(Directory.GetCurrentDirectory()),
                options => { options.MigrationsHistoryTable("__Migrations"); });
        return new AppDbMigrationContext(optionsBuilder.Options);
    }
}