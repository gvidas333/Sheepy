using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Repositories.Interfaces;

namespace server.Repositories;

public class MealRepository : IMealRepository
{
    private readonly ApplicationDbContext _context;

    public MealRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Meal meal)
    {
        await _context.Meals.AddAsync(meal);
        await _context.SaveChangesAsync();
    }

    public async Task<Meal?> GetByIdAsync(Guid mealId, Guid userId)
    {
        return await _context.Meals
            .Include(m => m.MealProducts)
                .ThenInclude(mp => mp.Product)
            .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == userId);
    }

    public async Task<IEnumerable<Meal>> GetMealsByUserAsync(Guid userId)
    {
        return await _context.Meals
            .Where(m => m.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Meal meal)
    {
        _context.Meals.Update(meal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Meal meal)
    {
        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
    }
}