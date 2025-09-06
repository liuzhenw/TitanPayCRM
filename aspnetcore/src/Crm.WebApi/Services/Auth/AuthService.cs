using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Crm.Accounts;
using Crm.Services.Auth.AuthStateStores;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Crm.Services.Auth;

public class AuthService(
    IConfiguration configuration,
    EmailVerificationCodeService codeService,
    UserManager userManager) :
    ITransientDependency
{
    public async Task<AuthToken> VerificationCodeSignInAsync(CodeSignInInput input)
    {
        await codeService.EnsureValidAsync(input.Email, input.Code);
        var user = await userManager.FindByEmailOrNameAsync(input.Email);
        return user is null
            ? throw new BusinessException(CrmErrorCodes.Accounts.NotFound)
            : GenerateToken(user);
    }

    public async Task<AuthToken> PasswordSignInAsync(PasswordSignInInput input)
    {
        var user = await userManager.FindByEmailOrNameAsync(input.UserName);
        if (user is null)
            throw new BusinessException(CrmErrorCodes.Accounts.NotFound);

        await userManager.PasswordAuthAsync(user, input.Password);
        return GenerateToken(user);
    }

    public async Task SendEmailVerificationCodeAsync(SendEmailVerificationCodeInput input)
    {
        var user = await userManager.FindByEmailOrNameAsync(input.Email);
        if (user is null)
            throw new BusinessException(CrmErrorCodes.Accounts.NotFound);
        
        await codeService.SendAsync(input.Email);
    }

    private AuthToken GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new(JwtClaimTypes.Subject, user.Id.ToString()),
            new(JwtClaimTypes.Name, user.Name)
        ];
        claims.AddRange(user.UserRoles.Select(role => new Claim(JwtClaimTypes.Role, role.RoleId)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authorization:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTimeOffset.Now.AddMinutes(int.Parse(configuration["Authorization:Expires"]!));
        var token = new JwtSecurityToken(
            configuration["Authorization:Issuer"]!,
            configuration["Authorization:Audience"]!,
            claims,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthToken(jwt, expires.ToUnixTimeSeconds());
    }
}

public record AuthToken(string AccessToken, long ExpiresIn);

public class PasswordSignInInput
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}

public class CodeSignInInput
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;
}

public class SendEmailVerificationCodeInput
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}