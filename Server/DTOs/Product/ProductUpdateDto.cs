using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Product;

public class ProductUpdateDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [StringLength(200)]
    public string? Description { get; set; }
    
    [Required]
    public Guid CategoryTypeId { get; set; }
}