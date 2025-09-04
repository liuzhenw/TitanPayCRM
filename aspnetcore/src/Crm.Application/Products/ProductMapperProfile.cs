using System;
using Astra.AutoMapper;
using Astra.AutoMapper.Converters;
using AutoMapper;

namespace Crm.Products;

public class ProductMapperProfile : Profile
{
    public ProductMapperProfile()
    {
        CreateMap<ProductCache, ProductBasicDto>()
            .ToUrl(d => d.ImageUrl, s => s.ImageUri);
    }
}

public class ProductValueConverter(IServiceProvider services)
    : EntityCacheValueConverter<ProductCache, ProductBasicDto, string>(services);