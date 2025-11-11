namespace SpotRent.Api.Dtos;

public class WorkingHoursDto
{
    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly OpenTime { get; set; }
    
    public TimeOnly CloseTime { get; set; }
    
    public bool IsClosed { get; set; }
}
