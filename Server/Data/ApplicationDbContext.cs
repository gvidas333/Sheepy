using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Data.EntityConfigurations;
using Server.Models;

namespace Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<CategoryType> CategoryTypes { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<MealProduct> MealProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CategoryTypeEntityConfiguration());
        builder.ApplyConfiguration(new MealEntityConfiguration());
        builder.ApplyConfiguration(new MealProductEntityConfiguration());
        builder.ApplyConfiguration(new ProductEntityConfiguration());
        builder.ApplyConfiguration(new ShoppingListEntityConfiguration());
        builder.ApplyConfiguration(new ShoppingListItemEntityConfiguration());
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}