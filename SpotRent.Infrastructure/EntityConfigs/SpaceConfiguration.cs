using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class SpaceConfiguration : IEntityTypeConfiguration<Space>
{
    public void Configure(EntityTypeBuilder<Space> builder)
    {
        builder.ToTable("space");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("space_id");

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(s => s.Type)
            .HasColumnName("type")
            .IsRequired();

        builder.Property(s => s.Capacity)
            .HasColumnName("capacity")
            .IsRequired();

        builder.Property(s => s.HourlyRate)
            .HasColumnName("hourly_rate")
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(s => s.Equipment)
            .HasColumnName("equipment")
            .HasMaxLength(1000);

        builder.Property(s => s.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500);

        builder.Property(s => s.Floor)
            .HasColumnName("floor")
            .HasMaxLength(50);

        builder.Property(s => s.RoomNumber)
            .HasColumnName("room_number")
            .HasMaxLength(20);

        builder.Property(s => s.IsAvailable)
            .HasColumnName("is_available")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.HasProjector).HasColumnName("has_projector");
        builder.Property(s => s.HasWhiteboard).HasColumnName("has_whiteboard");
        builder.Property(s => s.HasWiFi).HasColumnName("has_wifi");
        builder.Property(s => s.HasAirConditioning).HasColumnName("has_air_conditioning");

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
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
    }
}