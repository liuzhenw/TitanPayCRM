using Crm.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Crm.Admin;

public abstract class CrmAdminController : AbpControllerBase
{
    protected CrmAdminController()
    {
        LocalizationResource = typeof(CrmResource);
    }
}
