using FluentValidation;

namespace Crm.Admin.Settings;

public class SettingItemDto
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
    public string Type { get; set; } = null!;
    public string? Value { get; set; }
}

public class SettingUpdateInput
{
    public string Name { get; set; } = null!;
    public string? Value { get; set; }
}

public class SettingUpdateInputValidator : AbstractValidator<SettingUpdateInput>
{
    public SettingUpdateInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Value).MaximumLength(255);
    }
}