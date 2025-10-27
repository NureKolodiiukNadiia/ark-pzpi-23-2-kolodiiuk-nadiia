using Microsoft.AspNetCore.Identity;
using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; }
        
    public string LastName { get; set; }

    public Role Role { get; set; } = Role.User;

    public string GoogleId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();

    public ICollection<UserRefreshToken> RefreshTokens { get; set; }

    public ICollection<Space> Spaces { get; set; } = new List<Space>();
}
