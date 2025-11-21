namespace SpotRent.Api.Dtos.Subscriptions;

public class ChangePlanRequest
{
    public int SubscriptionId { get; set; }

    public int NewPlanId { get; set; }
}