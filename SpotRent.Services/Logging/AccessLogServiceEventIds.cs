using Microsoft.Extensions.Logging;

namespace SpotRent.Services.Logging;

internal static class AccessLogServiceEventIds
{
    internal static readonly EventId ErrorAccessLogRetrieval = new(7000, nameof(ErrorAccessLogRetrieval));

    internal static readonly EventId ErrorAccessLogsRetrieval = new(7001, nameof(ErrorAccessLogsRetrieval));

    internal static readonly EventId ErrorCreatingAccessLogEntry = new(7002, nameof(ErrorCreatingAccessLogEntry));
    
    internal static readonly EventId ErrorCreatingAccessLogEntryOwner = new(7003, nameof(ErrorCreatingAccessLogEntryOwner));
}
