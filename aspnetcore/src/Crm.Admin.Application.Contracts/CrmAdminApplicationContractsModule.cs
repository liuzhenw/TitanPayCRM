using Astra.Application.Contracts;
using Volo.Abp.Modularity;

namespace Crm.Admin;

[DependsOn(typeof(CrmDomainSharedModule),
    typeof(AstraApplicationContractsModule))]
public class CrmAdminApplicationContractsModule : AbpModule { }