namespace Server.Models;

public class Meal
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    
    public ApplicationUser User { get; set; }
    public ICollection<MealProduct> MealProducts { get; set; }
}