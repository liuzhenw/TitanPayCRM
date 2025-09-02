using Astra.AutoMapper;
using Astra.AutoMapper.Converters;
using AutoMapper;
using Crm.Admin.Accounts;
using Crm.Products;

namespace Crm.Admin.Products;

public class ProductMapperProfile : Profile
{
    public ProductMapperProfile()
    {
        CreateMap<ProductCache, ProductBasicDto>();
        CreateMap<Product, ProductDto>();

        CreateMap<ProductSaleLog, ProductSaleLogDto>()
            .ToValue<ProductSaleLog, ProductSaleLogDto, string, ProductBasicDto, ProductValueConverter>(
                d => d.Product, s => s.ProductId)
            .ToValue<ProductSaleLog, ProductSaleLogDto, Guid, UserBasicDto, UserValueConverter>(
                d => d.Customer, s => s.CustomerId);
        CreateMap<ProductSaleLogQueryInput, ProductSaleLogPagedParameter>();
    }
}

public class ProductValueConverter(IServiceProvider services)
    : EntityCacheValueConverter<ProductCache, ProductBasicDto, string>(services);