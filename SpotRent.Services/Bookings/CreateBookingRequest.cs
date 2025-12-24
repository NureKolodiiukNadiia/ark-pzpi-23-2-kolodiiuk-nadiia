namespace SpotRent.Services.Bookings;

public class CreateBookingRequest
{
    public int SpaceId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsValid()
        => SpaceId >= 1 && StartTime < EndTime && StartTime > DateTime.UtcNow && EndTime > DateTime.UtcNow;
}
