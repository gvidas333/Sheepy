using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Models;

namespace Server.Data.EntityConfigurations;

public class CategoryTypeEntityConfiguration : IEntityTypeConfiguration<CategoryType>
{
    public void Configure(EntityTypeBuilder<CategoryType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(x => x.Products)
            .WithOne(p => p.CategoryType)
            .HasForeignKey(p => p.CategoryTypeId);

        builder.HasData(
            new CategoryType { Id = Guid.Parse("ED80C4F6-D657-4ED4-854B-5254A7448A4E"), Name = "Duona" },
            new CategoryType { Id = Guid.Parse("0C8E9981-434B-4D39-A23F-26E2D1A4A49C"), Name = "Vaisiai ir Darzoves" }
        );
    }
}