namespace Server.Models;

public class ShoppingListItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid ShoppingListId { get; set; }
    public bool IsChecked { get; set; }
    
    public Product Product { get; set; }
    public ShoppingList ShoppingList { get; set; }
}