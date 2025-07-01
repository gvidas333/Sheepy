using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Repositories.Interfaces;

namespace Server.Repositories;

public class ShoppingListRepository :  IShoppingListRepository
{
    private readonly ApplicationDbContext _context;

    public ShoppingListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ShoppingList shoppingList)
    {
        await _context.ShoppingLists.AddAsync(shoppingList);
        await _context.SaveChangesAsync();
    }

    public async Task<ShoppingList?> GetByIdAsync(Guid shoppingListId, Guid userId)
    {
        return await _context.ShoppingLists
            .Include(sl => sl.ShoppingListItems)
                .ThenInclude(item => item.Product)
                    .ThenInclude(p => p.CategoryType)
            .FirstOrDefaultAsync(sl => sl.Id == shoppingListId && sl.UserId == userId);
    }

    public async Task<IEnumerable<ShoppingList>> GetShoppingListsByUserAsync(Guid userId)
    {
        return await _context.ShoppingLists
            .Where(sl => sl.UserId == userId)
            .OrderByDescending(sl => sl.CreatedAt)
            .ToListAsync();
    }

    public async Task<ShoppingList?> GetLatestForUserAsync(Guid userId)
    {
        return await _context.ShoppingLists
            .Where(sl => sl.UserId == userId)
            .OrderByDescending(sl => sl.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(ShoppingList shoppingList)
    {
        _context.ShoppingLists.Remove(shoppingList);
        await _context.SaveChangesAsync();
    }
}
