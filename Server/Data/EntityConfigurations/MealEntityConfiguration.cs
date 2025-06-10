using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models;

namespace Server.Data.EntityConfigurations;

public class MealEntityConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.HasOne(x => x.User)
            .WithMany(u => u.Meals)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.MealProducts)
            .WithOne(mp => mp.Meal)
            .HasForeignKey(mp => mp.MealId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}