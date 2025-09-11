using Astra;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Emailing;

namespace Crm.Admin.Settings;

[Authorize(Roles = AstraConsts.RootRole)]
public class EmailTestService(IEmailSender emailSender) : CrmAdminAppService, IEmailTestService
{
    public Task SendTestEmailAsync(TestEmailInput input)
    {
        const string html = "<html><body><h1>This is a test email from TitanPay.</h1></body></html>";
        return emailSender.SendAsync(input.ReceiverEmail, "TitanPay test email.", html);
    }
}