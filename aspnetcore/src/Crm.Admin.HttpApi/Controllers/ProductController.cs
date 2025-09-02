using Crm.Admin.Products;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("products")]
public class ProductController(IProductService productService) : CrmAdminController
{
    [HttpGet("{id}")]
    public Task<ProductDto> GetAsync(string id) => productService.GetAsync(id);

    [HttpGet]
    public Task<List<ProductDto>> GetListAsync() => productService.GetListAsync();

    [HttpPost]
    public Task<ProductDto> CreateAsync([FromBody] CreateProductInput input) => productService.CreateAsync(input);

    [HttpPut("{id}")]
    public Task<ProductDto> UpdateAsync(string id, [FromBody] UpdateProductInput input) => productService.UpdateAsync(id, input);

    [HttpDelete("{id}")]
    public Task DeleteAsync(string id) => productService.DeleteAsync(id);
}