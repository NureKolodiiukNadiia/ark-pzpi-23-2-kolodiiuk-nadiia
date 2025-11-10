using SpotRent.Domain.Enums;

namespace SpotRent.Services.Subscriptions;

public record SubscriptionInfo
{
    public int Id { get; init; }

    public int SubscriptionPlanId { get; init; }

    public string SubscriptionPlanName { get; init; }

    public SubscriptionStatus SubscriptionStatus { get; init; }

    public bool IsActive { get; init; }

    public DateTime StartedAt { get; init; }

    public DateTime? ExpiresAt { get; init; }
}