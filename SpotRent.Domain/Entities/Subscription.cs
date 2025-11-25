using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Subscription
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SubscriptionPlanId { get; set; }

    public decimal Price { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public SubscriptionStatus Status { get; set; }

    public int HoursUsed { get; set; }

    public decimal TotalAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public long TransactionId { get; set; }

    public DateTime? PaymentProcessedAt { get; set; }

    public DateTime PaymentCreatedAt { get; set; }

    public string PaymentFailureReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public User User { get; set; }

    public SubscriptionPlan SubscriptionPlan { get; set; }

    public bool IsActive()
    {
        var ended = DateTime.UtcNow >= EndDate;
        var cancelled = CancelledAt != null;
        var isPaid = PaymentProcessedAt != null
                     || PaymentStatus == PaymentStatus.Paid
                     || PaymentStatus == PaymentStatus.TestPaid;
        var includedHours = SubscriptionPlan?.IncludedHours ?? int.MaxValue;
        var hoursUsedUp = HoursUsed >= includedHours;

        return !ended && !cancelled && isPaid && !hoursUsedUp;
    }
}
