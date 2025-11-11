namespace SpotRent.Api.Dtos;

public class CreateSubscriptionDto
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal? Price { get; set; }
}
