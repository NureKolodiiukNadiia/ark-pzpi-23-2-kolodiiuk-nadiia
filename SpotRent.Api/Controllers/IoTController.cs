using Microsoft.AspNetCore.Mvc;
using SpotRent.Api.Dtos.Iot;
using SpotRent.Services.Interfaces;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/iot")]
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

    [HttpPost("handshake")]
    public async Task<IActionResult> Handshake(HandshakeRequest req)
    {
        return StatusCode(418);
        //device registration
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

    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmUnlock(int deviceId)
    {
        return StatusCode(418);
        // successful unlock
    }

    [HttpPost("fail")]
    public async Task<IActionResult> ReportFailedAttempt(int deviceId, string reason)
    {
        return StatusCode(418);
    }
}
