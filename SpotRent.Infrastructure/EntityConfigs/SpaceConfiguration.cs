using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class SpaceConfiguration : IEntityTypeConfiguration<Space>
{
    public void Configure(EntityTypeBuilder<Space> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id);

        builder.Property(s => s.OwnerId)
            .IsRequired();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.SpaceType)
            .IsRequired();

        builder.Property(s => s.AreaSqm);

        builder.Property(s => s.Capacity)
            .IsRequired();

        builder.Property(s => s.Capacity)
            .IsRequired();

        builder.Property(s => s.HourlyRate)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(s => s.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.AddressLine)
            .HasMaxLength(200);

        builder.Property(s => s.Floor)
            .HasMaxLength(50);

        builder.Property(s => s.House)
            .HasMaxLength(50);

        builder.Property(s => s.Street)
            .HasMaxLength(250);

        builder.Property(s => s.City)
            .HasMaxLength(250);

        builder.Property(s => s.Oblast)
            .HasMaxLength(200);

        builder.Property(s => s.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(s => s.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasMany(s => s.Bookings)
            .WithOne(b => b.Space)
            .HasForeignKey(b => b.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Devices)
            .WithOne(d => d.Space)
            .HasForeignKey(d => d.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Images)
            .WithOne(i => i.Space)
            .HasForeignKey(i => i.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.SpaceAttributes)
            .WithOne(sa => sa.Space)
            .HasForeignKey(sa => sa.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Owner)
            .WithMany(u => u.Spaces)
            .HasForeignKey(d => d.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
