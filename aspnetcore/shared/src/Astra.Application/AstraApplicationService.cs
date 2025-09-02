using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;

namespace Astra;

public class AstraApplicationService : ApplicationService
{
    protected Guid CurrentUserId => CurrentUser.Id ?? throw new AbpAuthorizationException();
}