namespace SpotRent.Api.Dto;

public class CreateSubscriptionDto
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal? Price { get; set; }
}
