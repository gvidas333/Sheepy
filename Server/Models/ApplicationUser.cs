using Microsoft.AspNetCore.Identity;

namespace Server.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<Product> Products { get; set; }
    public ICollection<Meal> Meals { get; set; }
    public ICollection<ShoppingList> ShoppingLists { get; set; }
    public ICollection<CategoryType> CategoryTypes { get; set; }
}