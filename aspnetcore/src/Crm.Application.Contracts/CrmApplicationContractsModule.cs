using Astra.Application.Contracts;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;

namespace Crm;

[DependsOn(
    typeof(CrmDomainSharedModule),
    typeof(AstraApplicationContractsModule)
)]
public class CrmApplicationContractsModule : AbpModule { }