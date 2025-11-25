namespace SpotRent.Services.Subscriptions;

public record SubscriptionHistory
{
    public IEnumerable<SubscriptionInfo> SubscriptionInfos { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalSubscriptions { get; init; }

    public int TotalPages { get; init; }
}
