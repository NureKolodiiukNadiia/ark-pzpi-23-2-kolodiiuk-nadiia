using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Attribute = SpotRent.Domain.Entities.Attribute;

namespace SpotRent.Infrastructure.EntityConfigs;

public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
{
    public void Configure(EntityTypeBuilder<Attribute> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.DataType)
            .HasMaxLength(50);

        builder.Property(a => a.Unit);

        builder.HasMany(a => a.AttributeValues)
            .WithOne(pa => pa.Attribute)
            .HasForeignKey(pa => pa.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
