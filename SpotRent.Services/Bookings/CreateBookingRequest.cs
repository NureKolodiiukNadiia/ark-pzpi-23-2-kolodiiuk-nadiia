namespace SpotRent.Services.Bookings;

public class CreateBookingRequest
{
    public int SpaceId { get; }

    public DateTime StartTime { get; }

    public DateTime EndTime { get; }

    public bool IsValid()
        => SpaceId >= 1 && StartTime < EndTime && StartTime > DateTime.UtcNow && EndTime < DateTime.UtcNow;
}
