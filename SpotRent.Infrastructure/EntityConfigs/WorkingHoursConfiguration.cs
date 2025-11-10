using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotRent.Domain.Entities;

namespace SpotRent.Infrastructure.EntityConfigs;

public class WorkingHoursConfiguration : IEntityTypeConfiguration<WorkingHours>
{
    public void Configure(EntityTypeBuilder<WorkingHours> builder)
    {
        builder.HasKey(wh => wh.Id);

        builder.Property(wh => wh.Id);

        builder.Property(wh => wh.SpaceId)
            .IsRequired();

        builder.Property(wh => wh.DayOfWeek)
            .IsRequired();

        builder.Property(wh => wh.OpenTime)
            .IsRequired();

        builder.Property(wh => wh.CloseTime)
            .IsRequired();

        builder.Property(wh => wh.IsClosed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(wh => wh.Space)
            .WithMany(s => s.WorkingHours)
            .HasForeignKey(wh => wh.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(wh => new { wh.SpaceId, wh.DayOfWeek })
            .IsUnique();
    }
}
