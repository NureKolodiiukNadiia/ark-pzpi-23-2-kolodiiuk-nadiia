using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id);

        builder.Property(p => p.BookingId);

        builder.Property(p => p.SubscriptionId);

        builder.Property(p => p.Amount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.TransactionId)
            .HasMaxLength(100);

        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.ProcessedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.FailureReason)
            .HasMaxLength(500);

        builder.HasOne(p => p.Booking)
            .WithOne(b => b.Payment)
            .HasForeignKey<Payment>(p => p.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Subscription)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
