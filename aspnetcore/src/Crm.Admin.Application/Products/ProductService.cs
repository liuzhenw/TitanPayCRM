using Crm.Admin.Permissions;
using Crm.Products;
using Microsoft.AspNetCore.Authorization;

namespace Crm.Admin.Products;

[Authorize(CrmPermissions.Products.Default)]
public class ProductService(IProductRepository repo) : CrmAdminAppService, IProductService
{
    public async Task<List<ProductDto>> GetListAsync()
    {
        var products = await repo.GetListAsync();
        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }

    public async Task<ProductDto> GetAsync(string id)
    {
        var product = await repo.GetAsync(id);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(CrmPermissions.Products.Create)]
    public async Task<ProductDto> CreateAsync(CreateProductInput input)
    {
        var product = new Product(input.Id, input.Name, input.Price)
        {
            ImageUri = input.ImageUri,
            Description = input.Description
        };

        await repo.InsertAsync(product);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(CrmPermissions.Products.Delete)]
    public async Task DeleteAsync(string id)
    {
        await repo.DeleteAsync(id);
    }

    [Authorize(CrmPermissions.Products.Update)]
    public async Task<ProductDto> UpdateAsync(string id, UpdateProductInput input)
    {
        var product = await repo.GetAsync(id);

        product.Name = input.Name;
        product.ImageUri = input.ImageUri;
        product.Description = input.Description;

        await repo.UpdateAsync(product);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }
}