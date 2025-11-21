using SpotRent.Domain.Enums;

namespace SpotRent.Services.Subscriptions;

public class CreateSubscriptionPlanDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public Duration Duration { get; set; }

    public int IncludedHours { get; set; }
}
