using AutoMapper;
using Microsoft.Extensions.Localization;
using Volo.Abp.Settings;

namespace Crm.Admin.Settings;

public class SettingMapperProfile : Profile
{
    public SettingMapperProfile()
    {
        CreateMap<SettingDefinition, SettingItemDto>().ConvertUsing<SettingDefinitionTypeConverter>();
    }
}

public class SettingDefinitionTypeConverter(
    IStringLocalizerFactory localizerFactory,
    ISettingProvider settingProvider) : 
    ITypeConverter<SettingDefinition, SettingItemDto>
{
    public SettingItemDto Convert(SettingDefinition source, SettingItemDto destination, ResolutionContext context)
    {
        var type = source.Properties.GetValueOrDefault("Type", "string");
        var value = settingProvider.GetOrNullAsync(source.Name).Result;
        return new SettingItemDto
        {
            Name = source.Name,
            DisplayName = source.DisplayName.Localize(localizerFactory),
            Description = source.Description?.Localize(localizerFactory) ?? string.Empty,
            Type = (string)type,
            Value = source.IsEncrypted ? "******" : value,
        };
    }
}