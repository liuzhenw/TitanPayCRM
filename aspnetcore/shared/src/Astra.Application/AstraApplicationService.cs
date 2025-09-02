using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;

namespace Astra;

public class AstraApplicationService : ApplicationService
{
    protected Guid CurrentUserId
    {
        get
        {
            if (CurrentUser.Id is null)
                throw new AbpAuthorizationException();
            
            return CurrentUser.Id.Value;
        }
    }
}