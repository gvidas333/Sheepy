namespace Server.Models;

public class MealProduct
{
    public Guid MealId { get; set; }
    public Guid ProductId { get; set; }
    public double Quantity { get; set; }
    
    public Meal Meal { get; set; }
    public Product Product { get; set; }
}