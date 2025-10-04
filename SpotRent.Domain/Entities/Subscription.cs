using System.ComponentModel.DataAnnotations;
using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Subscription
{
    public int Id { get; set; }
        
    public int UserId { get; set; }
        
    public int SubscriptionPlanId { get; set; }

    [Required]
    public decimal Price { get; set; }
        
    [Required]
    public DateTime StartDate { get; set; }
        
    [Required]
    public DateTime EndDate { get; set; }
        
    public SubscriptionStatus Status { get; set; }
        
    public int HoursUsed { get; set; } = 0;
        
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; }

    public SubscriptionPlan SubscriptionPlan { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
