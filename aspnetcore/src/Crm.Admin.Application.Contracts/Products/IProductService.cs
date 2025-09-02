using Volo.Abp.Application.Services;

namespace Crm.Admin.Products;

public interface IProductService : IApplicationService
{
    Task<ProductDto> GetAsync(string id);
    Task<List<ProductDto>> GetListAsync();
    Task<ProductDto> CreateAsync(CreateProductInput input);
    Task<ProductDto> UpdateAsync(string id, UpdateProductInput input);
    Task DeleteAsync(string id);
}