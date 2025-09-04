using Astra;
using Crm.Localization;
using Volo.Abp.Application.Services;

namespace Crm;

public abstract class CrmAppService : AstraApplicationService
{
    protected CrmAppService()
    {
        LocalizationResource = typeof(CrmResource);
        ObjectMapperContext = typeof(CrmApplicationModule);
    }
}
