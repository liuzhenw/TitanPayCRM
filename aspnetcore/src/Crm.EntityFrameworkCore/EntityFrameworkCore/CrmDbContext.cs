using Crm.Referrals;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.EntityFrameworkCore;

[ConnectionStringName(CrmDbProperties.ConnectionStringName)]
public class CrmDbContext(DbContextOptions<CrmDbContext> options) : AbpDbContext<CrmDbContext>(options), ICrmDbContext
{
    public DbSet<Referrer> Referrers { get; set; }
    public DbSet<ReferralRelation> ReferralRelations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCrm();
    }
}
