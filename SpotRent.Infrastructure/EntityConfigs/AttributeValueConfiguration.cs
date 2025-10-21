using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
{
    public void Configure(EntityTypeBuilder<AttributeValue> builder)
    {
        builder.HasKey(av => av.Id);

        builder.Property(av => av.Value)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(av => av.MinValue);

        builder.Property(av => av.MaxValue);

        builder.HasOne(av => av.Attribute)
            .WithMany(a => a.AttributeValues)
            .HasForeignKey(pa => pa.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}