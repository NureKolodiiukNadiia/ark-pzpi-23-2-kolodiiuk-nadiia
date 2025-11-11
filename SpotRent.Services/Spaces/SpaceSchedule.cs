namespace SpotRent.Services.Spaces;

public class SpaceSchedule
{
    public int SpaceId { get; set; }

    public IEnumerable<(DateTime startTime, DateTime endTime)> Bookings { get; set; }
}