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
}
