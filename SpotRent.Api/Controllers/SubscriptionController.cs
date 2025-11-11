using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> SubscribeAsync(SubscribeRequest request)
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

    [Authorize(Roles = "User")]
    [HttpGet("history/{userId:int}")]
    public async Task<ActionResult> GetHistoryAsync(
        int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _subscriptionService.GetSubscriptionHistoryAsync(userId, page, pageSize);
        if (result.Failure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new
        {
            data = result.Value.SubscriptionInfos,
            pagination = new
            {
                page = result.Value.Page,
                pageSize = result.Value.PageSize,
                totalItems = result.Value.TotalSubscriptions,
                totalPages = result.Value.TotalPages
            }
        });
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Subscription>> GetSubscriptionAsync(int id)
    {
        var subscriptionByIdResult = await _subscriptionService.GetSubscriptionByIdAsync(id);
        if (subscriptionByIdResult.Failure)
        {
            return BadRequest(subscriptionByIdResult.Error);
        }

        return Ok(subscriptionByIdResult.Value);
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
