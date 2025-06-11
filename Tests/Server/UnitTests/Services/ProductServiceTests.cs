using Moq;
using Server.Models;
using Server.Repositories.Interfaces;
using Server.Services;

namespace Tests.Server.UnitTests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _productService = new ProductService(_mockProductRepository.Object);
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductExists_ShouldReturnProductDto()
    {
        var productId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var categoryType = new CategoryType { Name = "Test Category", Id = new Guid() };

        var productEntity = CreateTestProduct(productId, userId, "Test Product", categoryType);

        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId, userId))
            .ReturnsAsync(productEntity);

        var result = await _productService.GetProductByIdAsync(productId, userId);

        Assert.NotNull(result);
        Assert.Equal(productEntity.Id, result.Id);
        Assert.Equal(productEntity.Name, result.Name);
        Assert.Equal(productEntity.CategoryType.Name, result.CategoryName);
    }

    [Fact]
    public async Task GetProductsForUserAsync_WhenProductsExist_ShouldReturnProductDtoList()
    {
        var userId = Guid.NewGuid();
        var categoryType = new CategoryType { Name = "Test Category", Id = new Guid() };

        var productEntities = new List<Product>()
        {
            CreateTestProduct(Guid.NewGuid(), userId, "Product 1", categoryType),
            CreateTestProduct(Guid.NewGuid(), userId, "Product 2", categoryType)
        };

        _mockProductRepository.Setup(repo => repo.GetProductsByUserAsync(userId))
            .ReturnsAsync(productEntities);

        var result = await _productService.GetProductsForUserAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Collection(result,
            item1 => Assert.Equal("Product 1", item1.Name),
            item2 => Assert.Equal("Product 2", item2.Name));
    }

    private Product CreateTestProduct(Guid id, Guid userId, string productName, CategoryType categoryType)
    {
        return new Product
        {
            Id = id,
            UserId = userId,
            Name = productName,
            CategoryTypeId = categoryType.Id,
            CategoryType = categoryType
        };
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Product)null);

        var result = await _productService.GetProductByIdAsync(Guid.NewGuid(), Guid.NewGuid());
        
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProductsForUserIdAsync_WhenProductsDoNotExist_ShouldReturnEmptyList()
    {
        var userId = Guid.NewGuid();

        var emptyProductList = new List<Product>();

        _mockProductRepository.Setup(repo => repo.GetProductsByUserAsync(userId))
            .ReturnsAsync(emptyProductList);

        var result = await _productService.GetProductsForUserAsync(userId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}