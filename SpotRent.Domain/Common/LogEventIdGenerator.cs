namespace SpotRent.Domain.Common;

public static class LogEventIdGenerator
{
    private static int _currEventId = 2000;

    public static int EventId { get => _currEventId; }

    public static int GetNextEventId() => Interlocked.Increment(ref _currEventId);
}
