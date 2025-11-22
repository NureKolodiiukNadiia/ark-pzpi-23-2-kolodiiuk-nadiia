namespace SpotRent.Api.Dtos.Iot;

public class DeviceStatusRequest
{
     public int DeviceId { get; set; }

    public bool IsOnline { get; set; }

    public string? StatusMessage { get; set; }
}
