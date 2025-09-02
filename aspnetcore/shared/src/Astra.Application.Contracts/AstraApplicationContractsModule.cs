using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Astra.Application.Contracts;

[DependsOn(typeof(AbpDddApplicationContractsModule))]
public class AstraApplicationContractsModule : AbpModule;