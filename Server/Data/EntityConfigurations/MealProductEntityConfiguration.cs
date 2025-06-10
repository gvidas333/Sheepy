using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models;

namespace Server.Data.EntityConfigurations;

public class MealProductEntityConfiguration : IEntityTypeConfiguration<MealProduct>
{
    public void Configure(EntityTypeBuilder<MealProduct> builder)
    {
        builder.HasKey(mp => new { mp.MealId, mp.ProductId });
        builder.Property(mp => mp.Quantity)
            .IsRequired();

        builder.HasOne(mp => mp.Meal)
            .WithMany(m => m.MealProducts)
            .HasForeignKey(mp => mp.MealId);

        builder.HasOne(mp => mp.Product)
            .WithMany(m => m.MealProducts)
            .HasForeignKey(mp => mp.ProductId);
    }
}