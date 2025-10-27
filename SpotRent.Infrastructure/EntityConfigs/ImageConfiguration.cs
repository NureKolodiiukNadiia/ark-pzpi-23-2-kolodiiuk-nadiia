using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id);

        builder.Property(i => i.ImageUrl)
            .HasMaxLength(500);

        builder.Property(i => i.SpaceId)
            .IsRequired();

        builder.HasOne(i => i.Space)
            .WithMany(s => s.Images)
            .HasForeignKey(b => b.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
