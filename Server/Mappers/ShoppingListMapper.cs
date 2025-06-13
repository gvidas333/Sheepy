using server.DTOs.ShoppingList;
using Server.Models;

namespace Server.Mappers;

public static class ShoppingListMapper
{
    public static ShoppingListDto ToDto(this ShoppingList shoppingList)
    {
        return new ShoppingListDto
        {
            Id = shoppingList.Id,
            Name = shoppingList.Name,
            CreatedAt = shoppingList.CreatedAt,
            Items = MapShoppingListItemsToDto(shoppingList.ShoppingListItems)
        };
    }

    private static List<ShoppingListItemDto> MapShoppingListItemsToDto(ICollection<ShoppingListItem>? items)
    {
        if (items == null || !items.Any())
        {
            return new List<ShoppingListItemDto>();
        }

        return items.Select(item => new ShoppingListItemDto
        {
            ProductId = item.ProductId,
            ProductName = item.Product?.Name ?? "N/A",
            Quantity = item.Quantity,
            IsChecked = item.IsChecked
        }).ToList();
    }

    public static IEnumerable<ShoppingListDto> ToDto(this IEnumerable<ShoppingList> shoppingLists)
    {
        return shoppingLists.Select(sl => sl.ToDto()).ToList();
    }
}