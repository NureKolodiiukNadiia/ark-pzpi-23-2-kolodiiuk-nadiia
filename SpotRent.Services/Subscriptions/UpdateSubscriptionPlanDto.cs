using SpotRent.Domain.Enums;

namespace SpotRent.Services.Subscriptions;

public class UpdateSubscriptionPlanDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }

    /* kinda todo
    public Duration Duration { get; set; }

    public int IncludedHours { get; set; }
    */
}
