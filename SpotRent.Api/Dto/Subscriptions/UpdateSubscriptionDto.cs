namespace SpotRent.Api.Dto.Subscriptions;

public class UpdateSubscriptionDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
}