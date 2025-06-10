namespace Server.Models;

public class Product
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryTypeId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public CategoryType CategoryType { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<MealProduct> MealProducts { get; set; }
    public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
}