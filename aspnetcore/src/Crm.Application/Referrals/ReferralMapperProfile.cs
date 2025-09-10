using System;
using Astra.AutoMapper;
using Astra.AutoMapper.Converters;
using AutoMapper;
using Crm.Products;
using Volo.Abp.AutoMapper;

namespace Crm.Referrals;

public class ReferralMapperProfile : Profile
{
    public ReferralMapperProfile()
    {
        CreateMap<ReferralLevelCache, ReferralLevelBasicDto>();
        CreateMap<ReferralLevel, ReferralLevelDto>();

        CreateMap<Referrer, ReferrerDto>()
            .ToValue<Referrer, ReferrerDto, string, ReferralLevelBasicDto, ReferralLevelConverter>(
                d => d.Level, s => s.LevelId);
        CreateMap<SaleStatistic, ReferrerSaleStatisticDto>()
            .ToValue<SaleStatistic, ReferrerSaleStatisticDto, string, ProductBasicDto, ProductValueConverter>(
                d => d.Product, s => s.ProductId);

        CreateMap<RecommendeeQueryModel, RecommendeeDto>()
            .ToValue<RecommendeeQueryModel, RecommendeeDto, string, ReferralLevelBasicDto, ReferralLevelConverter>(
                d => d.Level, s => s.LevelId)
            .ForMember(d => d.CreatedAt,
                o => o.MapFrom(s => s.CreatedAt.ToUnixTimeSeconds()));
        CreateMap<RecommendeeQueryInput, RecommendeeQueryModelPagedParameter>()
            .Ignore(x => x.AncestorId)
            .Ignore(x => x.RecommenderId);

        CreateMap<ReferrerRequest, ReferrerRequestDto>()
            .ToValue<ReferrerRequest, ReferrerRequestDto, string, ReferralLevelBasicDto, ReferralLevelConverter>(
                d => d.Level, s => s.LevelId);

        CreateMap<CommissionLog, CommissionLogDto>()
            .ToValue<CommissionLog, CommissionLogDto, string, ProductBasicDto, ProductValueConverter>(
                d => d.Product, s => s.ProductId)
            .ToValue<CommissionLog, CommissionLogDto, string, ReferralLevelBasicDto, ReferralLevelConverter>(
                d => d.Level, s => s.LevelId);
        CreateMap<CommissionLogQueryInput, CommissionLogPagedParameter>()
            .Ignore(x => x.ReceiverId)
            .Ignore(x => x.SaleLogId);

        CreateMap<WithdrawalRequest, WithdrawalRequestDto>()
            .ForMember(d => d.CreatedAt,
                o => o.MapFrom(d => d.CreatedAt.ToUnixTimeSeconds()))
            .ForMember(d => d.CompletedAt,
                o => o.MapFrom(d => d.CompletedAt == null ? 0 : d.CompletedAt.Value.ToUnixTimeSeconds()));
        CreateMap<WithdrawalRequestQueryInput, WithdrawalRequestPagedParameter>()
            .Ignore(x => x.ReferrerId);
    }
}

public class ReferralLevelConverter(IServiceProvider services)
    : EntityCacheValueConverter<ReferralLevelCache, ReferralLevelBasicDto, string>(services);