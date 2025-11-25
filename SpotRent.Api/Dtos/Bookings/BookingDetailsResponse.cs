using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Api.Dtos.Bookings;

public class BookingDetailsResponse
{
    public int Id { get; set; }

    public BookingStatus Status { get; set; }

    public int SpaceId { get; set; }

    public string SpaceName { get; set; }

    public Address Address { get; set; }

    public string Room { get; set; }

    public string ImageUrl { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal HourlyRate { get; set; }

    public decimal Total { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public DateTime? PaymentProcessedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public static BookingDetailsResponse MapBooking(Booking booking)
    {
        if (booking == null)
        {
            return null;
        }

        var space = booking.Space;

        return new BookingDetailsResponse
        {
            Id = booking.Id,
            Status = booking.Status,
            SpaceId = booking.SpaceId,
            SpaceName = space?.Name ?? string.Empty,
            Address = space?.Address,
            Room = space.Room ?? string.Empty,
            ImageUrl = space?.ImageUrl ?? string.Empty,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            HourlyRate = space.HourlyRate,
            Total = booking.TotalAmount,
            PaymentStatus = booking.PaymentStatus,
            PaymentProcessedAt = booking.PaymentProcessedAt,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt,
            CancelledAt = booking.CancelledAt
        };
    }
}
