using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Booking
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int SpaceId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CancelledAt { get; set; }

    public User User { get; set; }

    public Space Space { get; set; }

    public Payment Payment { get; set; }

    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();
}

