namespace SpotRent.Services.Subscriptions;

public class CreateSubscriptionDto
{
    public int UserId { get; set; }

    public int PlanId { get; set; }

    public bool IsValid() => UserId >= 1 && PlanId >= 1;
}
