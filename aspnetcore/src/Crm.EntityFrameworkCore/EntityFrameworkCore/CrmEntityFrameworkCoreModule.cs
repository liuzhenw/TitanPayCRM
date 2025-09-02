using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Crm.Products;

namespace Crm.EntityFrameworkCore;

[DependsOn(
    typeof(CrmDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CrmEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CrmDbContext>(options =>
        {
            options.AddDefaultRepositories<ICrmDbContext>(includeAllEntities: true);
            
            options.AddRepository<ProductSaleLog, ProductSaleLogRepository>();
            options.AddRepository<Product, ProductRepository>();
        });
    }
}
