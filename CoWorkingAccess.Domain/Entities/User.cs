using System.ComponentModel.DataAnnotations;
using CoWorkingAccess.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CoWorkingAccess.Domain.Entities;

public class User : IdentityUser<int>
{
    [Required]
    public string FirstName { get; set; }
        
    [Required]
    public string LastName { get; set; }
        
    [Required]
    public Role Role { get; set; } = Role.User;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
