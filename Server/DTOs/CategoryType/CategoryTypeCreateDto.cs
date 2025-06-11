using System.ComponentModel.DataAnnotations;

namespace server.DTOs.CategoryType;

public class CategoryTypeCreateDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
}