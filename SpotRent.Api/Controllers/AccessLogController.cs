using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Api.Dtos.Iot;
using SpotRent.Services.Interfaces;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessLogController : BaseController<AccessLogController>
{
    private readonly IAccessLogService _accessLogService;

    public AccessLogController(IAccessLogService accessLogService, ILogger<AccessLogController> logger)
        : base(logger)
    {
        _accessLogService = accessLogService;
    }

    [Authorize(Roles="User")]
    [HttpPost]
    public async Task<IActionResult> CreateLogEntry(LogAccessRequest request)
    {
        return StatusCode(418);
        /*
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
        */
    }

    [Authorize(Roles="Owner")]
    [HttpGet("space/{spaceId:int}")]
    public async Task<IActionResult> GetSpaceLogs(int spaceId)
    {
        return StatusCode(418);
    }

    [Authorize(Roles="Owner")]
    [HttpGet("owner/{spaceId:int}")]
    public async Task<IActionResult> GetOwnerLogs(int ownerId)
    {
        return StatusCode(418);
    }

    [Authorize(Roles="User")]
    [HttpGet("user/{spaceId:int}")]
    public async Task<IActionResult> GetUserLogs(int userId)
    {
        return StatusCode(418);
    }

    [Authorize(Roles="User")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLogById(int id)
    {
        return StatusCode(418);
    }
}
