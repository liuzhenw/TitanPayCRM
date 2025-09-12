using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Products;

public class ProductBasicDto : EntityDto<string>
{
    public string Name { get; set; } = null!;
    public string? ImageUri { get; set; }
}

public class ProductDto : ProductBasicDto
{
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public uint SalesVolume { get; set; }
    public decimal SalesRevenue { get; set; }
    public decimal TotalCommission { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class CreateProductInput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ImageUri { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

public class UpdateProductInput
{
    public string Name { get; set; } = null!;
    public string? ImageUri { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

public class CreateProductInputValidator : AbstractValidator<CreateProductInput>
{
    public CreateProductInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.ImageUri).MaximumLength(128);
        RuleFor(x => x.Description).MaximumLength(255);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

public class UpdateProductInputValidator : AbstractValidator<UpdateProductInput>
{
    public UpdateProductInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.ImageUri).MaximumLength(128);
        RuleFor(x => x.Description).MaximumLength(255);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}