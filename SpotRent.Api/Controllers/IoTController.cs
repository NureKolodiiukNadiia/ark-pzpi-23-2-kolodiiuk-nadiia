using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/iot")]
public class IoTController : ControllerBase
{
    private readonly IAccessLogService _accessLogService;

    private readonly ISpaceService _spaceService;
        
    public IoTController(IAccessLogService accessLogService, ISpaceService workspaceService)
    {
        _accessLogService = accessLogService;
        _spaceService = workspaceService;
    }
        
    [HttpPost("validate-access")]
    public async Task<IActionResult> ValidateAccess([FromBody] ValidateAccessRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        throw new NotImplementedException();
        // if (user == null)
        // {
        //     await _accessLogService.LogAccessAsync(request.UserId, request.DeviceId, AccessType.AccessDenied,
        //         errorMessage: "User not found");
        //     return Unauthorized(new { Message = "Access denied", Reason = "Invalid user" });
        // }
        //
        // var workspace = await _spaceService.GetSpaceByDeviceIdAsync(request.DeviceId);
        // if (workspace == null)
        // {
        //     await _accessLogService.LogAccessAsync(request.UserId, request.DeviceId, AccessType.AccessDenied,
        //         errorMessage: "Device not found");
        //     return BadRequest(new { Message = "Device not found" });
        // }
        //
        // var hasValidAccess = await _accessLogService.ValidateAccessAsync(request.UserId, request.DeviceId);
        // if (!hasValidAccess)
        // {
        //     await _accessLogService.LogAccessAsync(request.UserId, request.DeviceId, AccessType.AccessDenied,
        //         errorMessage: "No valid booking found");
        //     return Unauthorized(new { Message = "Access denied", Reason = "No valid booking" });
        // }
        //
        // await _accessLogService.LogAccessAsync(request.UserId, request.DeviceId, AccessType.Entry);
        //
        // return Ok(new { Message = "Access granted", UserId = request.UserId, DeviceId = request.DeviceId });
    }
        
    [HttpPost("log-access")]
    public async Task<IActionResult> LogAccess([FromBody] LogAccessRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
                
        var accessLog = await _accessLogService.LogAccessAsync(
            request.UserId, 
            request.DeviceId, 
            request.AccessType,
            request.BookingId,
            request.IsSuccessful,
            request.ErrorMessage
        );
            
        return Ok(new { Id = accessLog.Id, Timestamp = accessLog.Timestamp });
    }
        
    [HttpPost("device-status")]
    public async Task<IActionResult> UpdateDeviceStatus([FromBody] DeviceStatusRequest request)
    {
        // Log device status update
        var spaceResult = await _spaceService.GetSpaceByDeviceIdAsync(request.DeviceId);
        if (spaceResult == null)
            return BadRequest(new { Message = "Device not found" });
                
        // Update space availability based on device status if needed
        spaceResult.Value.IsAvailable = request.IsOnline;
        await _spaceService.UpdateSpaceAsync(spaceResult.Value);
            
        return Ok(new { Message = "Device status updated" });
    }
}
    
public class ValidateAccessRequest
{
    [Required]
    public int UserId { get; set; }
        
    [Required]
    public string DeviceId { get; set; }
        
    public string? AccessMethod { get; set; } // NFC, QR, Bluetooth
}
    
public class LogAccessRequest
{
    [Required]
    public int UserId { get; set; }
        
    [Required]
    public string DeviceId { get; set; }
        
    [Required]
    public AccessType AccessType { get; set; }
        
    public int? BookingId { get; set; }
        
    public bool IsSuccessful { get; set; } = true;
        
    public string? ErrorMessage { get; set; }
}
    
public class DeviceStatusRequest
{
    [Required]
    public string DeviceId { get; set; }
        
    [Required]
    public bool IsOnline { get; set; }
        
    public string? StatusMessage { get; set; }
}