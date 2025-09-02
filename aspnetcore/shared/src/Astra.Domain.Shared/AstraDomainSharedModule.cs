using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Astra;

[DependsOn(typeof(AbpDddDomainSharedModule))]
public class AstraDomainSharedModule : AbpModule;