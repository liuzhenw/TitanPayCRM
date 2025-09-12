using Astra.Common;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace Crm.Services.Auth.AuthStateStores;

public class EmailVerificationCodeService(
    ILogger<EmailVerificationCodeService> logger,
    IAuthStateStore stateStore,
    IEmailSender emailSender) : ITransientDependency
{
    public async Task SendAsync(string email)
    {
        if (!CheckHelper.IsEmailAddress(email))
            throw new BusinessException(CrmErrorCodes.Accounts.InvalidEmailAddress);

        var state = await stateStore.GenerateAsync(email.ToLowerInvariant());
        var body = EmailTemplate
            .Replace("{{appName}}", "TitanPay")
            .Replace("{{code}}", state.Code)
            .Replace("{{minutes}}", "30")
            .Replace("{{year}}", DateTime.UtcNow.Year.ToString())
            .Replace("{{email}}", email);
        await emailSender.QueueAsync(email, "Verification code", body);
#if DEBUG
        logger.LogWarning("Email verification code: {Code}", state.Code);
#endif
    }

    public async Task<bool> VerifyAsync(string email, string code)
    {
        var state = await stateStore.FindAsync(email);
        if (state is null || !state.Verify(code))
          return false;

        state.ExpireAt = DateTimeOffset.Now;
        await stateStore.SetAsync(email, state);
        return true;
    }

    private const string EmailTemplate =
        """
            <!doctype html>
            <html lang="en" style="margin:0;padding:0;">
              <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <meta http-equiv="x-ua-compatible" content="ie=edge" />
                <title>Your verification code</title>
                <style>
                  /* Basic email resets */
                  body,table,td,a{ -webkit-text-size-adjust:100%; -ms-text-size-adjust:100%; }
                  table,td{ mso-table-lspace:0pt; mso-table-rspace:0pt; }
                  img{ -ms-interpolation-mode:bicubic; }
                  body{ margin:0; padding:0; width:100% !important; height:100% !important; }
                  a{ text-decoration:none; }
                  /* Dark-mode friendly */
                  @media (prefers-color-scheme: dark) {
                    body, .bg { background:#0b0c0f !important; color:#e6e6e6 !important; }
                    .card { background:#14161a !important; border-color:#2b2f36 !important; }
                    .muted { color:#98a2b3 !important; }
                    .btn { background:#3b82f6 !important; color:#ffffff !important; }
                  }
                </style>
              </head>
              <body class="bg" style="margin:0; padding:0; background:#f4f6f8; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, 'Noto Sans', 'Apple Color Emoji','Segoe UI Emoji', 'Segoe UI Symbol', sans-serif; color:#101828;">
                <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="background:#f4f6f8; padding:24px 0;">
                  <tr>
                    <td align="center">
                      <table role="presentation" cellpadding="0" cellspacing="0" width="640" class="card" style="width:640px; max-width:100%; background:#ffffff; border:1px solid #e5e7eb; border-radius:12px; overflow:hidden;">
                        <tr>
                          <td style="padding:24px 28px 8px 28px;">
                            <div style="font-size:14px; font-weight:600; letter-spacing:.08em; text-transform:uppercase; color:#3b82f6;">Verification</div>
                            <h1 style="margin:8px 0 0; font-size:22px; line-height:1.4;">Your verification code</h1>
                          </td>
                        </tr>
                        <tr>
                          <td style="padding:8px 28px 0 28px;">
                            <p style="margin:0; font-size:16px; line-height:1.6;">Use the code below to verify your email address for <strong>{{appName}}</strong>.</p>
                          </td>
                        </tr>
                        <tr>
                          <td align="center" style="padding:16px 28px;">
                            <div class="btn" style="display:inline-block; font-size:28px; font-weight:700; letter-spacing:.24em; padding:14px 22px; border-radius:10px; background:#111827; color:#ffffff;">
                              {{code}}
                            </div>
                          </td>
                        </tr>
                        <tr>
                          <td style="padding:0 28px 8px 28px;">
                            <p class="muted" style="margin:0; font-size:14px; line-height:1.6; color:#475467;">This code expires in {{minutes}} minutes. For your security, do not share it with anyone.</p>
                          </td>
                        </tr>
                        <tr>
                          <td style="padding:8px 28px 24px 28px;">
                            <p class="muted" style="margin:0; font-size:13px; line-height:1.6; color:#667085;">If you didn’t request this, you can safely ignore this email.</p>
                          </td>
                        </tr>
                        <tr>
                          <td style="padding:16px 28px; background:#f8fafc; border-top:1px solid #e5e7eb;">
                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0">
                              <tr>
                                <td style="font-size:12px; color:#98a2b3;">&copy; {{year}} {{appName}}. All rights reserved.</td>
                                <td align="right" style="font-size:12px; color:#98a2b3;">
                                  Sent to {{email}}
                                </td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
              </body>
            </html>
        """;
}