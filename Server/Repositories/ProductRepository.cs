using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Repositories.Interfaces;
using Server.Models;

namespace server.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid productId, Guid userId)
    {
        return await _context.Products
            .Include((p => p.CategoryType))   
            .FirstOrDefaultAsync(p => p.Id == productId && p.UserId == userId);
    }

    public async Task<IEnumerable<Product>> GetProductsByUserAsync(Guid userId)
    {
        return await _context.Products
            .Where(p => p.UserId == userId)
            .Include(p => p.CategoryType)
            .ToListAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}