namespace SpotRent.Api.Logging;

internal static class AccessLogControllerEventIds
{
    internal static readonly EventId CreateAccessLogAttempt = new(8100, nameof(CreateAccessLogAttempt));

    internal static readonly EventId CreateAccessLogInvalid = new(8101, nameof(CreateAccessLogInvalid));

    internal static readonly EventId CreateAccessLogSuccess = new(8102, nameof(CreateAccessLogSuccess));

    internal static readonly EventId CreateAccessLogFailure = new(8103, nameof(CreateAccessLogFailure));

    internal static readonly EventId GetSpaceLogsAttempt = new(8104, nameof(GetSpaceLogsAttempt));

    internal static readonly EventId GetSpaceLogsInvalid = new(8105, nameof(GetSpaceLogsInvalid));

    internal static readonly EventId GetSpaceLogsSuccess = new(8106, nameof(GetSpaceLogsSuccess));

    internal static readonly EventId GetSpaceLogsFailure = new(8107, nameof(GetSpaceLogsFailure));

    internal static readonly EventId GetOwnerLogsAttempt = new(8108, nameof(GetOwnerLogsAttempt));

    internal static readonly EventId GetOwnerLogsInvalid = new(8109, nameof(GetOwnerLogsInvalid));

    internal static readonly EventId GetOwnerLogsSuccess = new(8110, nameof(GetOwnerLogsSuccess));

    internal static readonly EventId GetOwnerLogsFailure = new(8111, nameof(GetOwnerLogsFailure));

    internal static readonly EventId GetUserLogsAttempt = new(8112, nameof(GetUserLogsAttempt));

    internal static readonly EventId GetUserLogsInvalid = new(8113, nameof(GetUserLogsInvalid));

    internal static readonly EventId GetUserLogsSuccess = new(8114, nameof(GetUserLogsSuccess));

    internal static readonly EventId GetUserLogsFailure = new(8115, nameof(GetUserLogsFailure));

    internal static readonly EventId GetLogByIdAttempt = new(8116, nameof(GetLogByIdAttempt));

    internal static readonly EventId GetLogByIdInvalid = new(8117, nameof(GetLogByIdInvalid));

    internal static readonly EventId GetLogByIdSuccess = new(8118, nameof(GetLogByIdSuccess));

    internal static readonly EventId GetLogByIdFailure = new(8119, nameof(GetLogByIdFailure));

    internal static readonly EventId CreateAccessLogAttemptOwner = new (8120, nameof(CreateAccessLogAttemptOwner));
}
