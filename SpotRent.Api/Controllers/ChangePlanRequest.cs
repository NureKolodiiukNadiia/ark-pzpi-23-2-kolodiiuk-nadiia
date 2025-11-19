namespace SpotRent.Api.Controllers;

public class ChangePlanRequest
{
    public int SubscriptionId { get; set; }

    public int NewPlanId { get; set; }
}