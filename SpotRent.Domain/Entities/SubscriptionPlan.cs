using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public Duration Duration { get; set; }

    public int IncludedHours { get; set; }

    public bool IsActive { get; set; }

    public int? OwnerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }

    public User Owner { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
