using Crm.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Crm.Products;
using Crm.Referrals;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Crm.EntityFrameworkCore;

[DependsOn(
    typeof(CrmDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CrmEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureEntityOptions();
        AddDbContext(context.Services);
    }

    private void ConfigureEntityOptions()
    {
        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<User>(entity =>
                entity.DefaultWithDetailsFunc = user => user.Include(x => x.UserRoles));
        });
    }

    private static void AddDbContext(IServiceCollection services)
    {
        services.AddAbpDbContext<CrmDbContext>(options =>
        {
            options.AddDefaultRepositories<ICrmDbContext>(includeAllEntities: true);

            // Accounts repositories
            options.AddRepository<User, UserRepository>();
            options.AddRepository<Role, RoleRepository>();

            // Products repositories
            options.AddRepository<ProductSaleLog, ProductSaleLogRepository>();
            options.AddRepository<Product, ProductRepository>();

            // Referrals repositories
            options.AddRepository<ReferrerRequest, ReferrerRequestRepository>();
            options.AddRepository<CommissionLog, CommissionLogRepository>();
            options.AddRepository<ReferralLevel, ReferrerLevelRepository>();
            options.AddRepository<ReferralRelation, ReferralRelationRepository>();
            options.AddRepository<WithdrawalRequest, WithdrawalRequestRepository>();
        });
    }
}