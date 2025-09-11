using Astra;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Emailing;

namespace Crm.Admin.Settings;

[Authorize(Roles = AstraConsts.RootRole)]
public class EmailTestService(IEmailSender emailSender) : CrmAdminAppService, IEmailTestService
{
    public async Task SendTestEmailAsync(TestEmailInput input)
    {
        const string html = "<html><body><h1>This is a test email from TitanPay.</h1></body></html>";
        try
        {
            await emailSender.SendAsync(input.ReceiverEmail, "TitanPay test email.", html);
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(e.Message, innerException: e);
        }
    }
}