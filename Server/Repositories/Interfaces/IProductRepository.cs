using Server.Models;

namespace Server.Repositories.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid productId, Guid userId);
    Task<IEnumerable<Product>> GetProductsByUserAsync(Guid userId);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}