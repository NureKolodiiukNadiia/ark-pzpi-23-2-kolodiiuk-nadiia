using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Infrastructure.EntityConfigs;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.SpaceId)
            .IsRequired();

        builder.Property(b => b.StartTime)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(b => b.EndTime)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(b => b.Status)
            .IsRequired()
            .HasDefaultValue(BookingStatus.Pending);

        builder.Property(b => b.TotalAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(b => b.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(b => b.CancelledAt)
            .HasColumnType("timestamp with time zone");

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Space)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.SpaceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
