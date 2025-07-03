using Server.Models;

namespace server.DTOs.ShoppingList;

public class ShoppingListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> MealNames { get; set; }
    public List<ShoppingListItemDto> Items { get; set; } = new();
}

public class ShoppingListItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public double Quantity { get; set; }
    public bool IsChecked { get; set; }
}