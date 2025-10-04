using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("device");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("device_id");

        builder.Property(d => d.SpaceId).HasColumnName("space_id").IsRequired();

        builder.Property(d => d.DeviceId)
            .HasColumnName("device_identifier")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.DeviceName)
            .HasColumnName("device_name")
            .HasMaxLength(100);

        builder.Property(d => d.Status)
            .HasColumnName("status");

        builder.Property(d => d.LastHeartbeat)
            .HasColumnName("last_heartbeat")
            .HasColumnType("timestamp with time zone");

        builder.Property(d => d.IsOnline)
            .HasColumnName("is_online")
            .IsRequired();

        builder.Property(d => d.InstalledAt)
            .HasColumnName("installed_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(d => d.Space)
            .WithMany(s => s.Devices)
            .HasForeignKey(d => d.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.AccessLogs)
            .WithOne(a => a.Device)
            .HasForeignKey(a => a.DeviceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}