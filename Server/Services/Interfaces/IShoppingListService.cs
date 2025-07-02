using server.DTOs.ShoppingList;

namespace Server.Services.Interfaces;

public interface IShoppingListService
{
    Task<ShoppingListResponseDto> GenerateAsync(GenerateShoppingListDto dto, Guid userId);
    Task<ShoppingListDto> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<ShoppingListDto>> GetAllForUserAsync(Guid userId);
    Task<ShoppingListResponseDto?> GetLatestForUserAsync(Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}