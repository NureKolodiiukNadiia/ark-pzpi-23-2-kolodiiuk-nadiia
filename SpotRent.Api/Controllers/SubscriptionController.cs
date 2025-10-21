using AutoMapper;
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

    private readonly IMapper _mapper;

    public SubscriptionController(ISubscriptionService subscriptionService, IMapper mapper)
    {
        _subscriptionService = subscriptionService;
        _mapper = mapper;
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
        var spaceDeletionResult = await _subscriptionService.GetSubscriptionByIdAsync(id);
        if (spaceDeletionResult.Failure)
        {
            return BadRequest(spaceDeletionResult.Error);
        }

        return Ok(spaceDeletionResult.Value);
    }

    // POST /api/subscription
    [HttpPost]
    public async Task<ActionResult> CreateSubscriptionAsync([FromBody] CreateSubscriptionDto subscriptionDto)
    {
        var subscription = _mapper.Map<CreateSubscriptionDto, Subscription>(subscriptionDto);
        var spaceDeletionResult = await _subscriptionService.AddSubscriptionAsync(subscription);
        if (spaceDeletionResult.Failure)
        {
            return BadRequest(spaceDeletionResult.Error);
        }

        return Ok(spaceDeletionResult.Value);
    }

    // PUT /api/subscription/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateSubscriptionAsync(int id, [FromBody] UpdateSubscriptionDto subscriptionDto)
    {
        var subscription = _mapper.Map<UpdateSubscriptionDto, Subscription>(subscriptionDto);
        var updateSubscriptionResult = await _subscriptionService.UpdateSubscriptionAsync(subscription);
        if (updateSubscriptionResult.Failure)
        {
            return BadRequest(updateSubscriptionResult.Error);
        }

        return Ok(updateSubscriptionResult.Value);
    }

    // DELETE /api/subscription/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteSubscriptionAsync(int id)
    {
        var spaceDeletionResult = await _subscriptionService.DeleteSubscriptionAsync(id);
        if (spaceDeletionResult.Failure)
        {
            return BadRequest(spaceDeletionResult.Error);
        }

        return Ok();
    }

    // POST /api/subscription/user/{userId}
    [HttpPost("user/{userId:int}")]
    public async Task<ActionResult> SubscribeUserAsync(int userId, [FromBody] SubscribeUserRequest? request = null)
    {
        throw new NotImplementedException();
    }

    // GET /api/subscription/user/{userId}
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetUserSubscriptionsAsync(int userId)
    {
        throw new NotImplementedException();
    }
}

// Minimal request DTO used by SubscribeUserAsync - adjust or remove if project already contains a similar DTO.
public record SubscribeUserRequest
{
    public int PlanId { get; init; }
    public string? PaymentMethod { get; init; }
    public DateTime? StartDate { get; init; }
}
