using Astra;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Crm;

[DependsOn(
    typeof(CrmDomainModule),
    typeof(CrmApplicationContractsModule),
    typeof(AstraApplicationModule)
)]
public class CrmApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CrmApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CrmApplicationModule>(validate: true); });
    }
}