using System.Text.Json.Serialization;

namespace SpotRent.Domain.Entities;

public class WorkingHours
{
    public int Id { get; set; }

    public int SpaceId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly OpenTime { get; set; }

    public TimeOnly CloseTime { get; set; }

    public bool IsClosed { get; set; } = false;

    [JsonIgnore]
    public Space Space { get; set; }
}
