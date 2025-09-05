using Astra.AutoMapper;
using Astra.AutoMapper.Converters;
using AutoMapper;
using Crm.Admin.Accounts;
using Crm.Admin.Products;
using Crm.Referrals;
using Volo.Abp.AutoMapper;

namespace Crm.Admin.Referrals;

public class ReferralMapperProfile : Profile
{
    public ReferralMapperProfile()
    {
        CreateMap<ReferralLevelCache, ReferralLevelBasicDto>();
        CreateMap<ReferralLevel, ReferralLevelDto>();
        CreateMap<ReferralLevelUpdateInput, ReferralLevel>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .Ignore(x => x.ConcurrencyStamp)
            .ForMember(d => d.UpdatedAt, o => o.MapFrom(_ => DateTimeOffset.Now));
        CreateMap<ReferrerQueryInput, ReferrerPagedParameter>();

        CreateMap<Referrer, ReferrerDto>()
            .ToValue<Referrer, ReferrerDto, Guid, UserBasicDto, UserValueConverter>(
                d => d.User, s => s.Id)
            .ToValue<Referrer, ReferrerDto, string, ReferralLevelBasicDto, ReferralLevelValueConverter>(
                d => d.Level, s => s.LevelId);
        CreateMap<Referrer, ReferrerWithDetails>()
            .IncludeBase<Referrer, ReferrerDto>();
        CreateMap<ReferrerUpdateInput, Referrer>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .Ignore(x => x.ConcurrencyStamp)
            .ForMember(d => d.UpdatedAt, o => o.MapFrom(_ => DateTimeOffset.Now));
        CreateMap<SaleStatistic, ReferrerSaleStatisticDto>()
            .ToValue<SaleStatistic, ReferrerSaleStatisticDto, string, ProductBasicDto, ProductValueConverter>(
                d => d.Product, s => s.ProductId);

        CreateMap<ReferralRelation, ReferralRelationDto>();
        CreateMap<ReferralRelationUser, ReferralRelationUserDto>();
        CreateMap<ReferralRelationQueryInput, ReferralRelationPagedParameter>();

        CreateMap<ReferrerRequest, ReferrerRequestDto>()
            .ToValue<ReferrerRequest, ReferrerRequestDto, Guid, UserBasicDto, UserValueConverter>(
                d => d.User, s => s.Id)
            .ToValue<ReferrerRequest, ReferrerRequestDto, string, ReferralLevelBasicDto, ReferralLevelValueConverter>(
                d => d.Level, s => s.LevelId);
        CreateMap<ReferrerRequestQueryInput, ReferrerRequestPagedParameter>();
    }
}

public class ReferralLevelValueConverter(IServiceProvider services)
    : EntityCacheValueConverter<ReferralLevelCache, ReferralLevelBasicDto, string>(services);