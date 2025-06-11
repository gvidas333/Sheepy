namespace Server.Models;

public class CategoryType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    
    public ApplicationUser User { get; set; }
    public ICollection<Product> Products { get; set; }
}