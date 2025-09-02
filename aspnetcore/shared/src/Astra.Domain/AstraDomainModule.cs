using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Astra;

[DependsOn(
    typeof(AstraDomainSharedModule),
    typeof(AbpDddDomainModule))]
public class AstraDomainModule : AbpModule;