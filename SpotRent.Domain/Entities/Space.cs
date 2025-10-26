using System.ComponentModel.DataAnnotations;
using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Space
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int AddressId { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public SpaceType SpaceType { get; set; }

    public double AreaSqm { get; set; }

    public int Capacity { get; set; }

    [Required]
    public decimal HourlyRate { get; set; }

    public bool IsAvailable { get; set; } = true;
    
    public string AddressLine { get; set; }

    public string Floor { get; set; }

    public string House { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string Oblast { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; }

    public User Owner { get; set; } = new User();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<Device> Devices { get; set; } = new List<Device>();

    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();

    public ICollection<SpaceAttribute> SpaceAttributes { get; set; } = new List<SpaceAttribute>();
}
