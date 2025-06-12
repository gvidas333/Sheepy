using System.ComponentModel.DataAnnotations;

namespace server.DTOs.Meal;

public class MealUpdateDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [StringLength(200)]
    public string? Description { get; set; }

    public List<MealProductForCreateDto> Products { get; set; } = new();
}