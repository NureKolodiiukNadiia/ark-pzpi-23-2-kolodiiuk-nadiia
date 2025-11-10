namespace SpotRent.Api.Dto.Space;

public record AvailabilityResponseDto(int SpaceId)
{
    public IEnumerable<AvailabilityDayDto> Availability { get; init; } = [];
}
