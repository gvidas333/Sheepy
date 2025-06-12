namespace server.DTOs.Meal;

public class MealDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public List<ProductInMealDto> Products { get; set; } = new();
}

public class ProductInMealDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
}