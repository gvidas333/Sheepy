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

        builder.HasOne(ct => ct.User)
            .WithMany(u => u.CategoryTypes)
            .HasForeignKey(ct => ct.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}