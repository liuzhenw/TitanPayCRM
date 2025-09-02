using Crm.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Crm;

public abstract class CrmController : AbpControllerBase
{
    protected CrmController()
    {
        LocalizationResource = typeof(CrmResource);
    }
}
