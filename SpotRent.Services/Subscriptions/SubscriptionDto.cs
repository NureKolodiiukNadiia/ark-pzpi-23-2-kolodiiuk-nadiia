using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Services.Subscriptions;

public class SubscriptionDto
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public int SubscriptionPlanId { get; set; }

    public DateTime StartDate { get; set; }

    public SubscriptionStatus Status { get; set; }

    public DateTime EndDate { get; set; }

    public int HoursUsed { get; set; }

    public decimal TotalAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public DateTime? PaymentProcessedAt { get; set; }

    public string PaymentFailureReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public static SubscriptionDto MapSubscription(Subscription subscription)
    {
        return new SubscriptionDto
        {
            Id = subscription.Id,
            Price = subscription.Price,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            StartDate = subscription.StartDate,
            Status = subscription.Status,
            EndDate = subscription.EndDate,
            HoursUsed = subscription.HoursUsed,
            TotalAmount = subscription.TotalAmount,
            PaymentStatus = subscription.PaymentStatus,
            PaymentProcessedAt = subscription.PaymentProcessedAt,
            PaymentFailureReason = subscription.PaymentFailureReason,
            CreatedAt = subscription.CreatedAt,
            UpdatedAt = subscription.UpdatedAt
        };
    }
}
