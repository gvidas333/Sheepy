using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Repositories.Interfaces;

namespace server.Repositories;

public class CategoryTypeRepository : ICategoryTypeRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CategoryType categoryType)
    {
        await _context.CategoryTypes.AddAsync(categoryType);
        await _context.SaveChangesAsync();
    }

    public async Task<CategoryType?> GetByIdAsync(Guid categoryTypeId, Guid userId)
    {
        return await _context.CategoryTypes
            .FirstOrDefaultAsync(c => c.Id == categoryTypeId && c.UserId == userId);
    }

    public async Task<IEnumerable<CategoryType>> GetCategoryTypesByUserAsync(Guid userId)
    {
        return await _context.CategoryTypes
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task DeleteAsync(CategoryType categoryType)
    {
        _context.CategoryTypes.Remove(categoryType);
        await _context.SaveChangesAsync();
    }
}