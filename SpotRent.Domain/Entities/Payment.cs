using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int? BookingId { get; set; }

    public int? SubscriptionId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    //todo: check the data type
    [StringLength(100)]
    public string TransactionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    [StringLength(500)]
    public string FailureReason { get; set; }

    public Booking Booking { get; set; }

    public Subscription Subscription { get; set; }
}
