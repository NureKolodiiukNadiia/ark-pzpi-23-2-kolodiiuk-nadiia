namespace SpotRent.Api.Logging;

internal static class SpacesControllerEventIds
{
    internal static readonly EventId GetSpacesAttempt = new(5200, "GetSpacesAttempt");

    internal static readonly EventId GetSpacesInvalid = new(5201, "GetSpacesInvalid");

    internal static readonly EventId GetSpacesSuccess = new(5202, "GetSpacesSuccess");

    internal static readonly EventId GetSpacesFailure = new(5203, "GetSpacesFailure");

    internal static readonly EventId GetSpaceAttempt = new(5204, "GetSpaceAttempt");

    internal static readonly EventId GetSpaceInvalid = new(5205, "GetSpaceInvalid");

    internal static readonly EventId GetSpaceSuccess = new(5206, "GetSpaceSuccess");

    internal static readonly EventId GetSpaceFailure = new(5207, "GetSpaceFailure");

    internal static readonly EventId CreateSpaceAttempt = new(5208, "CreateSpaceAttempt");

    internal static readonly EventId CreateSpaceInvalid = new(5209, "CreateSpaceInvalid");

    internal static readonly EventId CreateSpaceSuccess = new(5210, "CreateSpaceSuccess");

    internal static readonly EventId CreateSpaceFailure = new(5211, "CreateSpaceFailure");

    internal static readonly EventId UpdateSpaceAttempt = new(5212, "UpdateSpaceAttempt");

    internal static readonly EventId UpdateSpaceInvalid = new(5213, "UpdateSpaceInvalid");

    internal static readonly EventId UpdateSpaceSuccess = new(5214, "UpdateSpaceSuccess");

    internal static readonly EventId UpdateSpaceFailure = new(5215, "UpdateSpaceFailure");

    internal static readonly EventId DeleteSpaceAttempt = new(5216, "DeleteSpaceAttempt");

    internal static readonly EventId DeleteSpaceInvalid = new(5217, "DeleteSpaceInvalid");

    internal static readonly EventId DeleteSpaceSuccess = new(5218, "DeleteSpaceSuccess");

    internal static readonly EventId DeleteSpaceFailure = new(5219, "DeleteSpaceFailure");

    internal static readonly EventId GetSpaceScheduleAttempt = new(5220, "GetSpaceScheduleAttempt");

    internal static readonly EventId GetSpaceScheduleInvalid = new(5221, "GetSpaceScheduleInvalid");

    internal static readonly EventId GetSpaceScheduleSuccess = new(5222, "GetSpaceScheduleSuccess");

    internal static readonly EventId GetSpaceScheduleFailure = new(5223, "GetSpaceScheduleFailure");

    internal static readonly EventId GetAvailableSpacesAttempt = new(1120, nameof(GetAvailableSpacesAttempt));

    internal static readonly EventId GetAvailableSpacesInvalid = new(1121, nameof(GetAvailableSpacesInvalid));

    internal static readonly EventId GetAvailableSpacesSuccess = new(1122, nameof(GetAvailableSpacesSuccess));

    internal static readonly EventId GetAvailableSpacesFailure = new(1123, nameof(GetAvailableSpacesFailure));
}
