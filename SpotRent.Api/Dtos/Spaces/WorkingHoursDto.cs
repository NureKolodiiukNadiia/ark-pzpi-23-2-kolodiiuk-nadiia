using SpotRent.Domain.Entities;

namespace SpotRent.Api.Dtos.Spaces;

public class WorkingHoursDto
{
    public int Id { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly OpenTime { get; set; }

    public TimeOnly CloseTime { get; set; }

    public bool IsClosed { get; set; }

    public static WorkingHoursDto Map(WorkingHours wh)
    {
        if (wh is null)
        {
            return null;
        }
        
        return new WorkingHoursDto
        {
            Id = wh.Id,
            DayOfWeek = wh.DayOfWeek,
            OpenTime = wh.OpenTime,
            CloseTime = wh.CloseTime,
            IsClosed = wh.IsClosed
        };
    }
}
