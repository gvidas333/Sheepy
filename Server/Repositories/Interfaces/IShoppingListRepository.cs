using Server.Models;

namespace Server.Repositories.Interfaces;

public interface IShoppingListRepository
{
    Task AddAsync(ShoppingList shoppingList);
    Task<ShoppingList?> GetByIdAsync(Guid shoppingListId, Guid userId);
    Task<IEnumerable<ShoppingList>> GetShoppingListsByUserAsync(Guid userId);
    Task<ShoppingList?> GetLatestForUserAsync(Guid userId);
    Task DeleteAsync(ShoppingList shoppingList);
}