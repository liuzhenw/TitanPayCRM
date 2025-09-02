using Astra;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Crm.Admin;

[DependsOn(
    typeof(CrmDomainModule),
    typeof(CrmAdminApplicationContractsModule),
    typeof(AstraApplicationModule)
)]
public class CrmAdminApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CrmAdminApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CrmAdminApplicationModule>(validate: true); });
    }
}