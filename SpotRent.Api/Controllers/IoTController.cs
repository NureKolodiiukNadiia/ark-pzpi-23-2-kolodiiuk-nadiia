using Microsoft.AspNetCore.Mvc;
using SpotRent.Api.Dtos.Iot;
using SpotRent.Services.Interfaces;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IoTController : BaseController<IoTController>
{
    private readonly ISmartLockService _smartLockService;

    private readonly IQrScannerService _qrScannerService;

    public IoTController(
        ISmartLockService smartLockService,
        IQrScannerService qrScannerService,
        ILogger<IoTController> logger)
        : base(logger)
    {
        _smartLockService = smartLockService;
        _qrScannerService = qrScannerService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterDevice(DeviceRegistrationRequest request)
    {
        return StatusCode(418);
    }

    [HttpPost("device-status")]
    public async Task<IActionResult> UpdateDeviceStatus(DeviceStatusRequest request)
    {
        return StatusCode(418);
    }

    [HttpPost("unlock")]
    public async Task<IActionResult> Unlock(int deviceId, int userId, string qrToken)
    {
        return StatusCode(418);
    }

    [HttpPost("lock/{deviceId:int}")]
    public async Task<IActionResult> Lock(int deviceId)
    {
        return StatusCode(418);
        // successful unlock
    }
}
