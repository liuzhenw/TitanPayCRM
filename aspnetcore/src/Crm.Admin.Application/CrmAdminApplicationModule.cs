using Astra;
using Crm.Admin.TitanPay;
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
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CrmAdminApplicationModule>(validate: true); });
        context.Services.AddAutoMapperObjectMapper<CrmAdminApplicationModule>();
        context.Services.AddHttpClient<TitanPayApiClient>().AddStandardResilienceHandler();
    }
}