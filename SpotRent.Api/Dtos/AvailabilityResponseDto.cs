namespace SpotRent.Api.Dtos;

public record AvailabilityResponseDto(int SpaceId)
{
    public IEnumerable<AvailabilityDayDto> Availability { get; init; } = [];
}
