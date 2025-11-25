namespace SpotRent.Api.Logging;

internal static class BookingsControllerEventIds
{
    internal static readonly EventId CreateBookingAttempt = new(5000, "CreateBookingAttempt");

    internal static readonly EventId CreateBookingInvalid = new(5001, "CreateBookingInvalid");

    internal static readonly EventId CreateBookingSuccess = new(5002, "CreateBookingSuccess");

    internal static readonly EventId CreateBookingFailure = new(5003, "CreateBookingFailure");

    internal static readonly EventId GetUserBookingsHistoryAttempt = new(5004, "GetUserBookingsHistoryAttempt");

    internal static readonly EventId GetUserBookingsHistoryInvalid = new(5005, "GetUserBookingsHistoryInvalid");

    internal static readonly EventId GetUserBookingsHistorySuccess = new(5006, "GetUserBookingsHistorySuccess");

    internal static readonly EventId GetUserBookingsHistoryFailure = new(5007, "GetUserBookingsHistoryFailure");

    internal static readonly EventId GetUserActiveBookingsAttempt = new(5008, "GetUserActiveBookingsAttempt");

    internal static readonly EventId GetUserActiveBookingsInvalid = new(5009, "GetUserActiveBookingsInvalid");

    internal static readonly EventId GetUserActiveBookingsSuccess = new(5010, "GetUserActiveBookingsSuccess");

    internal static readonly EventId GetUserActiveBookingsFailure = new(5011, "GetUserActiveBookingsFailure");

    internal static readonly EventId GetOwnerBookingsAttempt = new(5012, "GetOwnerBookingsAttempt");

    internal static readonly EventId GetOwnerBookingsInvalid = new(5013, "GetOwnerBookingsInvalid");

    internal static readonly EventId GetOwnerBookingsSuccess = new(5014, "GetOwnerBookingsSuccess");

    internal static readonly EventId GetOwnerBookingsFailure = new(5015, "GetOwnerBookingsFailure");

    internal static readonly EventId GetBookingsAttempt = new(5016, "GetBookingsAttempt");

    internal static readonly EventId GetBookingsSuccess = new(5017, "GetBookingsSuccess");

    internal static readonly EventId GetBookingsFailure = new(5018, "GetBookingsFailure");

    internal static readonly EventId GetBookingAttempt = new(5019, "GetBookingAttempt");

    internal static readonly EventId GetBookingInvalid = new(5020, "GetBookingInvalid");

    internal static readonly EventId GetBookingSuccess = new(5021, "GetBookingSuccess");

    internal static readonly EventId GetBookingFailure = new(5022, "GetBookingFailure");

    internal static readonly EventId CancelBookingAttempt = new(5023, "CancelBookingAttempt");
}
