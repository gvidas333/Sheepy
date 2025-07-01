namespace Server.Models;

public class ShoppingList
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> MealNames { get; set; } = new();
    public ApplicationUser User { get; set; }
    public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
}