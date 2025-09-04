using Astra.Application.Contracts;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;

namespace Crm;

[DependsOn(
    typeof(CrmDomainSharedModule),
    typeof(AstraApplicationContractsModule),
    typeof(AbpFluentValidationModule)
)]
public class CrmApplicationContractsModule : AbpModule { }