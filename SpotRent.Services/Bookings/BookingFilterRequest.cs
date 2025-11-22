using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Services.Bookings;

public class BookingFilterRequest
{
    public string Role { get; set; }

    public int UserId { get; set; }

    public int? SpaceId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public BookingStatus? Status { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    public Func<IQueryable<Booking>, IOrderedQueryable<Booking>> OrderBy { get; set; }

    public int SkipCount { get; set; }

    public int? TakeCount { get; set; }
}
