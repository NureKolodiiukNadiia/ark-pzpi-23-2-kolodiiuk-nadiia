using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SpaceId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public decimal TotalAmount { get; set; }

    public Duration BookingType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CancelledAt { get; set; }

    public User User { get; set; }

    public Space Space { get; set; }

    public Payment Payment { get; set; }
}
