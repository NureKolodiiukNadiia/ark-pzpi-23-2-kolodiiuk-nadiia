using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class SpaceAttributeConfiguration : IEntityTypeConfiguration<SpaceAttribute>
{
    public void Configure(EntityTypeBuilder<SpaceAttribute> builder)
    {
        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.Value)
            .HasMaxLength(500);

        builder.HasOne(pa => pa.Attribute)
            .WithMany(a => a.SpaceAttributes)
            .HasForeignKey(pa => pa.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pa => pa.Space)
            .WithMany(p => p.SpaceAttributes)
            .HasForeignKey(pa => pa.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
