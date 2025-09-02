using Astra;
using Volo.Abp.Modularity;

namespace Crm;

[DependsOn(
    typeof(AstraDomainModule),
    typeof(CrmDomainSharedModule)
)]
public class CrmDomainModule : AbpModule
{

}
