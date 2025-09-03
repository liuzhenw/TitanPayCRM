using Crm.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(AuthService service) : CrmController
{
    [HttpPost("sign-in/password")]
    public Task<AuthToken> PasswordSignInAsync(PasswordSignInInput input) => service.PasswordSignInAsync(input);
    
    [HttpPost("sign-in/code")]
    public Task<AuthToken> CodeSignInAsync(CodeSignInInput input) => service.VerificationCodeSignInAsync(input);

    [HttpPost("verification-code")]
    public Task SendEmailCodeAsync(SendEmailVerificationCodeInput input) => service.SendEmailVerificationCodeAsync(input);
}