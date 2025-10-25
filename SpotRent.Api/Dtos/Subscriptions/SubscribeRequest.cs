namespace SpotRent.Api.Controllers;

public record SubscribeRequest
{
    public int UserId { get; init; }

    public int PlanId { get; init; }
}
