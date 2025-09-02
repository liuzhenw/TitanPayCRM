using Astra;
using Crm.Localization;
using Volo.Abp.Application.Services;

namespace Crm.Admin;

public abstract class CrmAdminAppService : AstraApplicationService
{
    protected CrmAdminAppService()
    {
        LocalizationResource = typeof(CrmResource);
        ObjectMapperContext = typeof(CrmAdminApplicationModule);
    }
}
