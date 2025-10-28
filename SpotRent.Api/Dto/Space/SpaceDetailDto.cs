namespace SpotRent.Api.Dto.Space;

public record SpaceDetailDto
{
    public int Id { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public string SpaceType { get; init; }

    public int Capacity { get; init; }

    public double AreaSqm { get; init; }

    public decimal HourlyRate { get; init; }

    public decimal DailyRate { get; init; }

    public IEnumerable<string> Images { get; init; } = Array.Empty<string>();
    
    public IEnumerable<WorkingHourDto> WorkingHours { get; init; } = Array.Empty<WorkingHourDto>();
    
    public double Rating { get; init; }

    public int ReviewCount { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }
}
