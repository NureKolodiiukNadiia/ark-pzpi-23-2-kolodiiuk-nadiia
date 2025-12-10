using Microsoft.Extensions.Logging;

namespace SpotRent.Services.Logging;

internal static class SmartLockServiceEventIds
{
    internal static readonly EventId RegisterDevice = new(8200, nameof(RegisterDevice));

    internal static readonly EventId UpdateDeviceStatus = new(8201, nameof(UpdateDeviceStatus));

    internal static readonly EventId UnlockAttempt = new(8202, nameof(UnlockAttempt));

    internal static readonly EventId UnlockSuccess = new(8203, nameof(UnlockSuccess));

    internal static readonly EventId UnlockFailure = new(8204, nameof(UnlockFailure));

    internal static readonly EventId LockDevice = new(8205, nameof(LockDevice));

    internal static readonly EventId Error = new(8299, nameof(Error));
}
