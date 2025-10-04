using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Infrastructure.EntityConfigs;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("booking_id");

        builder.Property(b => b.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(b => b.SpaceId).HasColumnName("space_id").IsRequired();

        builder.Property(b => b.StartTime)
            .HasColumnName("start_time")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(b => b.EndTime)
            .HasColumnName("end_time")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(b => b.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasDefaultValue(BookingStatus.Pending);

        builder.Property(b => b.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(b => b.BookingType)
            .HasColumnName("booking_type")
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(b => b.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(b => b.CancelledAt)
            .HasColumnName("cancelled_at")
            .HasColumnType("timestamp with time zone");

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Space)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.SpaceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Payment)
            .WithOne(p => p.Booking)
            .HasForeignKey<Payment>(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
