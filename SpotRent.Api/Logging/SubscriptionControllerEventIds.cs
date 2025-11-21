namespace SpotRent.Api.Logging;

internal static class SubscriptionControllerEventIds
{
    internal static readonly EventId Subscribe = new (2000, "Subscribe");

    internal static readonly EventId GetSubscriptionById = new (2001, "GetSubscriptionById");

    internal static readonly EventId GetSubscriptionHistory = new (2002, "GetSubscriptionHistory");

    internal static readonly EventId GetMySubscription = new (2003, "GetMySubscription");

    internal static readonly EventId ChangeSubscription = new (2004, "ChangeSubscription");

    internal static readonly EventId CancelSubscription = new (2005, "CancelSubscription");
}
