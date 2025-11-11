using Microsoft.AspNetCore.Mvc;
using SpotRent.Api.Dtos;
using SpotRent.Domain.Entities;
using SpotRent.Services.Interfaces;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    // GET /api/subscription?userId=1&status=active&limit=50&offset=0
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions(
        [FromQuery] int? userId,
        [FromQuery] string? status,
        [FromQuery] int limit = 50,
        [FromQuery] int offset = 0)
    {
        throw new NotImplementedException();
    }

    // GET /api/subscription/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Subscription>> GetSubscriptionAsync(int id)
    {
        throw new NotImplementedException();
    }

    // POST /api/subscription
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult> CreateSubscriptionAsync([FromBody] CreateSubscriptionDto subscriptionDto)
    {
        var result = await _subscriptionService.SubscribeAsync(request.UserId, request.PlanId);
        if (result.Failure)
        {
            return BadRequest(result.Error);
        }

        return StatusCode(StatusCodes.Status201Created, new { id = result.Value });
    }


    [Authorize(Roles = "User")]
    [HttpGet("me/{userId:int}")]
    public async Task<ActionResult> GetMySubscriptionAsync(int userId)
    {
        var result = await _subscriptionService.GetCurrentUserSubscriptionAsync(userId);

        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        return Ok(result.Value);
    }

    // PUT /api/subscription/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateSubscriptionAsync(int id, [FromBody] UpdateSubscriptionDto subscriptionDto)
    {
        throw new NotImplementedException();
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetPlansAsync()
    {
        var result = await _subscriptionService.GetPlansAsync();
        if (result.Failure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { data = result.Value });
    }

    [HttpGet("plans/{id:int}")]
    public async Task<IActionResult> GetPlanAsync(int id)
    {
        var result = await _subscriptionService.GetPlanByIdAsync(id);
        if (result.Failure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    // GET /api/subscription/user/{userId}
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetUserSubscriptionsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "User")]
    [HttpPost("{id:int}/change")]
    public async Task<ActionResult> ChangeSubscriptionAsync(ChangePlanRequest request)
    {
        //todo: check user
        var result = await _subscriptionService.ChangeSubscriptionAsync(request.SubscriptionId, request.NewPlanId);
        if (result.Failure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = "User")]
    [HttpPost("{userId:int}/{subscriptionId:int}/cancel")]
    public async Task<ActionResult> CancelSubscriptionAsync(int userId, int subscriptionId)
    {
        //todo:check user
        var result = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
        if (result.Failure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}

// Minimal request DTO used by SubscribeUserAsync - adjust or remove if project already contains a similar DTO.
public record SubscribeUserRequest
{
    public int PlanId { get; init; }
    public string? PaymentMethod { get; init; }
    public DateTime? StartDate { get; init; }
}