using System;
using FluentValidation;
using Volo.Abp.Application.Dtos;

namespace Crm.Users;

public class UserDto : EntityDto<Guid>
{
    public decimal TotalConsumption { get; set; }
    public uint ConsumptionCount { get; set; }
    public string Email { get; set; } = null!;
    public bool HasPassword { get; set; }
    public string? AvatarUrl { get; set; }
}

public record UserCheckDto(string Email, bool Exist);

public class ChangePasswordInput
{
    public string? OldPassword { get; set; }
    public string NewPassword { get; set; } = null!;
}

public class ChangePasswordInputValidator : AbstractValidator<ChangePasswordInput>
{
    public ChangePasswordInputValidator()
    {
        RuleFor(x => x.OldPassword).MinimumLength(6);
        RuleFor(x => x.NewPassword).MinimumLength(6);
    }
}