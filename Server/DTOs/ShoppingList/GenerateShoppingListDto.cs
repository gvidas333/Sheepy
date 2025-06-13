using System.ComponentModel.DataAnnotations;

namespace server.DTOs.ShoppingList;

public class GenerateShoppingListDto
{
    [Required] public string ListName { get; set; } = "New Shopping List";

    public List<Guid> MealIds { get; set; } = new();
    public List<Guid> ProductIds { get; set; } = new();
}