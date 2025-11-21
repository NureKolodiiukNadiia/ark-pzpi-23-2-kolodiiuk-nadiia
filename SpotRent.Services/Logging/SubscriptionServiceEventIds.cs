using Microsoft.Extensions.Logging;
using SpotRent.Services.Subscriptions;

namespace SpotRent.Services.Logging;

internal static class SubscriptionServiceEventIds
{
    internal static readonly EventId Subscribe = new(2101, nameof(SubscriptionService.SubscribeAsync));

    internal static readonly EventId GetCurrentUserSubscription = new(2102, nameof(SubscriptionService.GetCurrentUserSubscriptionAsync));

    internal static readonly EventId GetSubscriptionHistory = new(2103, nameof(SubscriptionService.GetSubscriptionHistoryAsync));

    internal static readonly EventId GetSubscriptionById = new(2104, nameof(SubscriptionService.GetSubscriptionByIdAsync));

    internal static readonly EventId ChangeSubscription = new(2105, nameof(SubscriptionService.ChangeSubscriptionAsync));

    internal static readonly EventId CancelSubscription = new(2106, nameof(SubscriptionService.CancelSubscriptionAsync));
}
