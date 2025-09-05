using Crm.Accounts;
using Crm.Products;
using Crm.Referrals;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.EntityFrameworkCore;

[ConnectionStringName(CrmDbProperties.ConnectionStringName)]
public class CrmDbContext(DbContextOptions<CrmDbContext> options) : AbpDbContext<CrmDbContext>(options), ICrmDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductSaleLog> ProductSaleLogs { get; set; }
    
    public DbSet<Referrer> Referrers { get; set; }
    public DbSet<ReferralLevel> ReferralLevels { get; set; }
    public DbSet<ReferrerRequest>  ReferrerRequests { get; set; }
    public DbSet<ReferralRelation> ReferralRelations { get; set; }
    public DbSet<CommissionLog> CommissionLogs { get; set; }
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCrm();
    }
}
