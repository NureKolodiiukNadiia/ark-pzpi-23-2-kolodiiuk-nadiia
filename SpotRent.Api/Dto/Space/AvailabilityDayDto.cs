namespace SpotRent.Api.Dto.Space;

public record AvailabilityDayDto(DateTime Date, bool IsAvailable, WorkingHourDto WorkingHours)
{
    public IEnumerable<TimeSlotDto> BookedSlots { get; init; } = Array.Empty<TimeSlotDto>();

    public IEnumerable<TimeSlotDto> AvailableSlots { get; init; } = Array.Empty<TimeSlotDto>();
}