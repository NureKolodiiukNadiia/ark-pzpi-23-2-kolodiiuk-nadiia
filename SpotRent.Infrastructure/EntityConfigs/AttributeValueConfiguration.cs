using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
{
    public void Configure(EntityTypeBuilder<AttributeValue> builder)
    {
        builder.Property(av => av.Id);

        builder.Property(av => av.Value)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(av => av.MinValue);

        builder.Property(av => av.MaxValue);

        builder.Property(av => av.SpaceId)
            .IsRequired();

        builder.Property(av => av.AttributeId)
            .IsRequired();

        builder.HasOne(av => av.Space)
            .WithMany(s => s.AttributeValues)
            .HasForeignKey(av => av.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
