namespace Server.Models;

public class CategoryType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<Product> Products { get; set; }
}