using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace Astra.Application.Contracts;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpFluentValidationModule)
)]
public class AstraApplicationContractsModule : AbpModule;