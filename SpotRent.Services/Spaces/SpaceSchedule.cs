namespace SpotRent.Services.Spaces;

public class SpaceSchedule
{
    public int SpaceId { get; set; }

    public IEnumerable<StartEndTime> Bookings { get; set; }
}
