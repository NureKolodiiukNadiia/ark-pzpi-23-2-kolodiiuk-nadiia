using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id);

        builder.Property(a => a.Building)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.City)
            .HasMaxLength(300);

        builder.Property(a => a.Region)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(a => a.Spaces)
            .WithOne(s => s.Address)
            .HasForeignKey(s => s.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
