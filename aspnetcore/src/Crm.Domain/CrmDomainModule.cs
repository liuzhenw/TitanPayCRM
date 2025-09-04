using System;
using Astra;
using Crm.Accounts;
using Crm.Products;
using Crm.Referrals;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Modularity;

namespace Crm;

[DependsOn(
    typeof(AstraDomainModule),
    typeof(CrmDomainSharedModule),
    typeof(AbpAutoMapperModule)
)]
public class CrmDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureMapper(context.Services);
        AddEntityCaches(context.Services);
    }
    private static void ConfigureMapper(IServiceCollection services)
    {
        services.AddAutoMapperObjectMapper<CrmDomainModule>();
        services.Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CrmDomainModule>(validate: true); });
    }
    private static void AddEntityCaches(IServiceCollection services)
    {
        services.AddEntityCache<User, UserCache, Guid>();
        services.AddEntityCache<Role, RoleCache, string>();

        services.AddEntityCache<Product, ProductCache, string>();

        services.AddEntityCache<ReferralLevel, ReferralLevelCache, string>();
    }
}