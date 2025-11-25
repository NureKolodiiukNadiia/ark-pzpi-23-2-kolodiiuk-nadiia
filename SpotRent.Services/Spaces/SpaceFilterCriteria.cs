using SpotRent.Domain.Enums;

namespace SpotRent.Services.Spaces;

public record SpaceFilterCriteria
{
    public SpaceType? SpaceType { get; init; }

    public int? MinCapacity { get; init; }

    public int? MaxCapacity { get; init; }

    public double? MinAreaSqm { get; init; }

    public double? MaxAreaSqm { get; init; }

    public decimal? MinHourlyRate { get; init; }

    public decimal? MaxHourlyRate { get; init; }

    public string City { get; init; }

    public string Attributes { get; init; }
}
