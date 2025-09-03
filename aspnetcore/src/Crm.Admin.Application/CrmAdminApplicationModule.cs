using Astra;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Crm.Admin;

[DependsOn(
    typeof(CrmDomainModule),
    typeof(AbpSettingManagementDomainModule),
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