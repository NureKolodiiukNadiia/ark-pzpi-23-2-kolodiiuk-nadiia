using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class AccessLogConfiguration : IEntityTypeConfiguration<AccessLog>
{
    public void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id);

        builder.Property(a => a.UserId);

        builder.Property(a => a.SpaceId);

        builder.Property(a => a.DeviceId);

        builder.Property(a => a.AccessType)
            .IsRequired();

        builder.Property(a => a.Timestamp)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(a => a.IsSuccessful)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.ErrorMessage)
            .HasMaxLength(500);

        builder.HasOne(a => a.User)
            .WithMany(u => u.AccessLogs)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.Space)
            .WithMany(s => s.AccessLogs)
            .HasForeignKey(a => a.SpaceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.Device)
            .WithMany(d => d.AccessLogs)
            .HasForeignKey(a => a.DeviceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}