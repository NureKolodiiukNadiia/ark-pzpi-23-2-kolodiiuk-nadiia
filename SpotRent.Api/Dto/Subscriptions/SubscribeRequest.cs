namespace SpotRent.Api.Dto.Subscriptions;

public record SubscribeRequest
{
    public int UserId { get; init; }

    public int PlanId { get; init; }
}
