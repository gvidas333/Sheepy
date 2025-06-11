using Server.DTOs.Product;
using Server.Models;

namespace Server.Mappers;

public static class ProductMapper
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryName = product.CategoryType?.Name ?? "N/A"
        };
    }

    public static IEnumerable<ProductDto> ToDto(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToDto());
    }
    
    public static Product ToEntity(this ProductCreateDto productCreateDto)
    {
        return new Product
        {
            Name = productCreateDto.Name,
            Description = productCreateDto.Description,
            CategoryTypeId = productCreateDto.CategoryTypeId
        };
    }

    public static void ToEntity(this ProductUpdateDto productUpdateDto, Product productToUpdate)
    {
        productToUpdate.Name = productUpdateDto.Name;
        productToUpdate.Description = productUpdateDto.Description;
        productToUpdate.CategoryTypeId = productUpdateDto.CategoryTypeId;
    }
}