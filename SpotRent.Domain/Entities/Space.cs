using System.ComponentModel.DataAnnotations;
using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Space
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public SpaceType Type { get; set; }

    public int Capacity { get; set; }

    [Required]
    public decimal HourlyRate { get; set; }

    public string Equipment { get; set; }

    public string ImageUrl { get; set; }

    [StringLength(50)]
    public string Floor { get; set; }

    [StringLength(20)]
    public string RoomNumber { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool HasProjector { get; set; }

    public bool HasWhiteboard { get; set; }

    public bool HasWiFi { get; set; }

    public bool HasAirConditioning { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<Device> Devices { get; set; } = new List<Device>();

    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();
}
