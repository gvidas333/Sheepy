using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models;

namespace Server.Data.EntityConfigurations;

public class ShoppingListEntityConfiguration : IEntityTypeConfiguration<ShoppingList>
{
    public void Configure(EntityTypeBuilder<ShoppingList> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("now()");

        builder.HasOne(x => x.User)
            .WithMany(u => u.ShoppingLists)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ShoppingListItems)
            .WithOne(s => s.ShoppingList)
            .HasForeignKey(s => s.ShoppingListId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}