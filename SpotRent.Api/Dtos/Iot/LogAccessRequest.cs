using SpotRent.Domain.Enums;

namespace SpotRent.Api.Dtos.Iot;

public class LogAccessRequest
{
    public int UserId { get; set; }

    public string DeviceId { get; set; }

    public AccessType AccessType { get; set; }

    public int? BookingId { get; set; }

    public bool IsSuccessful { get; set; } = true;

    public string? ErrorMessage { get; set; }
}
