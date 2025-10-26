using System.ComponentModel.DataAnnotations;
using SpotRent.Domain.Enums;

namespace SpotRent.Domain.Entities;

public class Device
{
    public int Id { get; set; }

    public int SpaceId { get; set; }

    [StringLength(100)]
    public string DeviceName { get; set; }

    public LockStatus? Status { get; set; }

    public bool IsOnline { get; set; }

    public DateTime InstalledAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Space Space { get; set; }

    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();
}
