using Microsoft.Extensions.Logging;
using SpotRent.Services.Spaces;

namespace SpotRent.Services.Logging;

internal static class SpaceServiceEventIds
{
    internal static readonly EventId FilterSpaces = new(6100, nameof(SpaceService.FilterSpacesAsync));

    internal static readonly EventId GetSpaceById = new(6101, nameof(SpaceService.GetSpaceByIdAsync));

    internal static readonly EventId GetAvailableSpaces = new(6102, nameof(SpaceService.GetAvailableSpacesAsync));

    internal static readonly EventId CreateSpace = new(6103, nameof(SpaceService.CreateSpaceAsync));

    internal static readonly EventId UpdateSpace = new(6104, nameof(SpaceService.UpdateSpaceAsync));

    internal static readonly EventId DeleteSpace = new(6105, nameof(SpaceService.DeleteSpaceAsync));

    internal static readonly EventId GetSpaceSchedule = new(6106, nameof(SpaceService.GetSpaceScheduleAsync));

    internal static readonly EventId CheckAvailability = new(6107, nameof(SpaceService.IsSpaceAvailableAsync));
}
