using CoWorkingAccess.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoWorkingAccess.Infrastructure.EntityConfigs;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("subscription_plan");

        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).HasColumnName("subscription_plan_id");

        builder.Property(sp => sp.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sp => sp.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(sp => sp.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(sp => sp.Duration)
            .HasColumnName("duration")
            .IsRequired();

        builder.Property(sp => sp.IncludedHours)
            .HasColumnName("included_hours")
            .IsRequired();

        builder.Property(sp => sp.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(sp => sp.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(sp => sp.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");

        builder.HasMany(sp => sp.Subscriptions)
            .WithOne(s => s.SubscriptionPlan)
            .HasForeignKey(s => s.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}