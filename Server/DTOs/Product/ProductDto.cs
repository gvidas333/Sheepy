namespace Server.DTOs.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string CategoryName { get; set; }
}