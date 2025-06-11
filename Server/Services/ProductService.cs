using Server.DTOs.Product;
using Server.Mappers;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> AddProductAsync(ProductCreateDto productCreateDto, Guid userId)
    {
        var product = productCreateDto.ToEntity();
        product.UserId = userId;
        await _productRepository.AddAsync(product);
        return product.ToDto();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid productId, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(productId, userId);
        return product?.ToDto();
    }

    public async Task<IEnumerable<ProductDto>> GetProductsForUserAsync(Guid userId)
    {
        var products = await _productRepository.GetProductsByUserAsync(userId);
        return products.ToDto();
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateDto productUpdateDto, Guid userId)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productUpdateDto.Id, userId);
        
        if (existingProduct == null)
        {
            return false;
        }
        
        productUpdateDto.ToEntity(existingProduct);
        return true;
    }

    public async Task<bool> DeleteProductAsync(Guid productId, Guid userId)
    {
        var productToDelete = await _productRepository.GetByIdAsync(productId, userId);

        if (productToDelete == null)
        {
            return false;
        }

        await _productRepository.DeleteAsync(productToDelete);
        return true;
    }
}