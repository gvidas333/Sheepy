using System.ComponentModel.DataAnnotations;

namespace server.DTOs.Meal;

public class MealCreateDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [StringLength(200)]
    public string? Description { get; set; }

    public List<MealProductForCreateDto> Products { get; set; } = new();
}

public class MealProductForCreateDto
{
    [Required]
    public Guid ProductId { get; set; }
    
    [Required]
    [Range(0.1, 10000)]
    public double Quantity { get; set; }
}