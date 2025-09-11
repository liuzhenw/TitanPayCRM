using Volo.Abp.Application.Services;

namespace Crm.Admin.Settings;

public interface IEmailTestService : IApplicationService
{
    Task SendTestEmailAsync(TestEmailInput input);
}

public class TestEmailInput
{
    public string ReceiverEmail { get; set; } = null!;
}