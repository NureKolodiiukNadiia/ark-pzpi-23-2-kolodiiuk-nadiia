namespace SpotRent.Services.Subscriptions;

public class UpdateSubscriptionPlanDto
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }

    /* kinda todo
    public Duration Duration { get; set; }

    public int IncludedHours { get; set; }
    */
}
