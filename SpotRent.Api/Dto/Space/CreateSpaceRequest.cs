using SpotRent.Domain.Enums;

namespace SpotRent.Api.Dto.Space;

public record CreateSpaceRequest
{
    public string Name { get; init; }

    public string Description { get; init; }

    public SpaceType SpaceType { get; init; }

    public int Capacity { get; init; }

    public double AreaSqm { get; init; }

    public decimal HourlyRate { get; init; }

    public IEnumerable<int> EquipmentIds { get; init; } = [];

    public IEnumerable<WorkingHourDto> WorkingHours { get; init; } = [];
}
