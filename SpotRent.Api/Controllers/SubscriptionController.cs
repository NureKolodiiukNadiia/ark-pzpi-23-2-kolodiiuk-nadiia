using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Api.Dtos;
using SpotRent.Domain.Entities;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Subscriptions;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : BaseController<SubscriptionController>
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService, ILogger<SubscriptionController> logger)
        : base(logger)
    {
        _subscriptionService = subscriptionService;
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult> CreateSubscriptionAsync([FromBody] CreateSubscriptionDto subscriptionDto)
    {
        var result = await _subscriptionService.SubscribeAsync(subscriptionDto.UserId, subscriptionDto.PlanId);
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }

        return StatusCode(StatusCodes.Status201Created, new { id = result.Value });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Subscription>> GetSubscriptionByIdAsync(int id)
    {
        throw new NotImplementedException();
        var result = await _subscriptionService.GetSubscriptionByIdAsync(id);
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

        return StatusCode(StatusCodes.Status200OK, result.Value);
    }

    [Authorize(Roles = "User")]
    [HttpPost("{id:int}/change")]
    public async Task<ActionResult> ChangeSubscriptionAsync(ChangePlanRequest request)
    {
        //todo: check user
        var result = await _subscriptionService.ChangeSubscriptionAsync(request.SubscriptionId, request.NewPlanId);
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }

        return StatusCode(StatusCodes.Status200OK, result.Value);
    }

    [Authorize(Roles = "User")]
    [HttpPost("{userId:int}/{subscriptionId:int}/cancel")]
    public async Task<ActionResult> CancelSubscriptionAsync(int userId, int subscriptionId)
    {
        //todo:check user
        var result = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }

        return StatusCode(StatusCodes.Status200OK);
    }
}