using SpotRent.Services.Payment;

namespace SpotRent.Services.Subscriptions;

public class SubscriptionCreationResponse
{
    public int SubscriptionId { get; set; }

    public LiqPayPaymentData LiqPayPaymentData { get; set; }
}
