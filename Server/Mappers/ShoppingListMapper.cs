using server.DTOs.ShoppingList;
using Server.Models;

namespace Server.Mappers;

public static class ShoppingListMapper
{
    public static ShoppingListDto ToDto(this ShoppingList shoppingList, List<string> categoryOrder = null)
    {
        return new ShoppingListDto
        {
            Id = shoppingList.Id,
            Name = shoppingList.Name,
            CreatedAt = shoppingList.CreatedAt,
            MealNames = shoppingList.MealNames,
            Items = MapShoppingListItemsToDto(shoppingList.ShoppingListItems, categoryOrder)
        };
    }

    private static List<ShoppingListItemDto> MapShoppingListItemsToDto(ICollection<ShoppingListItem>? items, List<string> categoryOrder)
    {
        if (items == null || !items.Any())
        {
            return new List<ShoppingListItemDto>();
        }

        var itemsToMap = items.AsEnumerable();

        if (categoryOrder != null && categoryOrder.Any())
        {
            itemsToMap = itemsToMap.OrderBy(item =>
            {
                if (item.Product?.CategoryType.Name == null) return int.MaxValue;

                var index = categoryOrder.IndexOf(item.Product.CategoryType.Name);
                return index == -1 ? int.MaxValue : index;
            });
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