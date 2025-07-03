namespace server.DTOs.ShoppingList;

public class ShoppingListResponseDto
{
    public ShoppingListDto List { get; set; }
    public List<string> CategoryOrder { get; set; }
}