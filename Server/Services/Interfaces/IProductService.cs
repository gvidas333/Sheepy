using Server.DTOs.Product;

namespace Server.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto> AddProductAsync(ProductCreateDto productCreateDto, Guid userId);
    Task<ProductDto?> GetProductByIdAsync(Guid productId, Guid userId);
    Task<IEnumerable<ProductDto>> GetProductsForUserAsync(Guid userId);
    Task<bool> UpdateProductAsync(ProductUpdateDto productUpdateDto, Guid userId);
    Task<bool> DeleteProductAsync(Guid productId, Guid userId);
}