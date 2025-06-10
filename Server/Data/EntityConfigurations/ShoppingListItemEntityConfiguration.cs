using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models;

namespace Server.Data.EntityConfigurations;

public class ShoppingListItemEntityConfiguration : IEntityTypeConfiguration<ShoppingListItem>
{
    public void Configure(EntityTypeBuilder<ShoppingListItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.HasOne(x => x.Product)
            .WithMany(p => p.ShoppingListItems)
            .HasForeignKey(x => x.ProductId);

        builder.HasOne(x => x.ShoppingList)
            .WithMany(sl => sl.ShoppingListItems)
            .HasForeignKey(x => x.ShoppingListId);
    }
}