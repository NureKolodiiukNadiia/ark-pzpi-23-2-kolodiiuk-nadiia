using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class AccessLogConfiguration : IEntityTypeConfiguration<AccessLog>
{
    public void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        builder.ToTable("access_log");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("access_log_id");

        builder.Property(a => a.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(a => a.SpaceId).HasColumnName("space_id").IsRequired();
        builder.Property(a => a.DeviceId).HasColumnName("device_id").IsRequired();

        builder.Property(a => a.AccessType)
            .HasColumnName("access_type")
            .IsRequired();

        builder.Property(a => a.Timestamp)
            .HasColumnName("timestamp")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(a => a.IsSuccessful)
            .HasColumnName("is_successful")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.ErrorMessage)
            .HasColumnName("error_message")
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
