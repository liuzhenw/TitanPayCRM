using Astra.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Astra;

[DependsOn(typeof(AstraApplicationContractsModule),
    typeof(AstraDomainModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
)]
public class AstraApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AstraApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<AstraApplicationModule>(validate: true); });
    }
}