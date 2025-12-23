namespace SpotRent.Api.Dtos.Iot;

public class UnlockDeviceRequest
{
    public int DeviceId { get; set; }

    public int UserId { get; set; }

    public string QrCode { get; set; }

    public bool IsOwnerOverride { get; set; }
}
