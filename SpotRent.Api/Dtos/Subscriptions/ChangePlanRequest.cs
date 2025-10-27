namespace SpotRent.Api.Controllers;

public record ChangePlanRequest
{
    public int NewPlanId { get; init; }

    public int UserId { get; init; }

    public int SubscriptionId { get; init; }
}