using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Api.Logging;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Extensions;
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

    // [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult> CreateSubscriptionAsync([FromBody] CreateSubscriptionDto subscriptionDto)
    {
        if (!subscriptionDto.IsValid())
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Id of user or plan is not valid");
        }

        var result = await _subscriptionService.SubscribeAsync(subscriptionDto.UserId, subscriptionDto.PlanId);

        result
            .OnSuccess(() =>
            {
                Log(LogLevel.Information, SubscriptionControllerEventIds.Subscribe,
                    "Subscription created for user {userId}, plan {planId}.",
                    subscriptionDto.UserId, subscriptionDto.PlanId);
            })
            .OnFailure(() =>
            {
                Log(LogLevel.Error, SubscriptionControllerEventIds.Subscribe,
                    "Error creating subscription for user {userId}. Error: {error}",
                    subscriptionDto.UserId, result.Error);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status400BadRequest, result.Error)
            : StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Subscription>> GetSubscriptionByIdAsync(int id)
    {
        if (id < 1)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Id is not valid");
        }

        var result = await _subscriptionService.GetSubscriptionByIdAsync(id);

        result
            .OnSuccess(() =>
            {
                Log(LogLevel.Information, SubscriptionControllerEventIds.GetSubscriptionById,
                    "Retrieved subscription with id {id}.", id);
            })
            .OnFailure(() =>
            {
                Log(LogLevel.Error, SubscriptionControllerEventIds.GetSubscriptionById,
                    "Error retrieving subscription with id {id}. Error: {error}", id, result.Error);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status500InternalServerError, result.Error)
            : StatusCode(StatusCodes.Status200OK, SubscriptionDto.MapSubscription(result.Value));
    }

    [HttpGet("history/{userId:int}")]
    public async Task<IActionResult> GetSubscriptionHistory(int userId)
    {
        if (userId < 1)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Id is not valid");
        }

        var result = await _subscriptionService.GetSubscriptionHistoryAsync(userId);

        result
            .OnSuccess(() =>
            {
                Log(LogLevel.Information, SubscriptionControllerEventIds.GetSubscriptionHistory,
                    "Retrieved subscription history for user {userId}.", userId);
            })
            .OnFailure(() =>
            {
                Log(LogLevel.Error, SubscriptionControllerEventIds.GetSubscriptionHistory,
                    "Error retrieving subscription history for user {userId}. Error: {error}",
                    userId, result.Error);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status500InternalServerError, result.Error)
            : StatusCode(StatusCodes.Status200OK, result.Value);
    }

    // [Authorize(Roles = "User")]
    [HttpGet("me/{userId:int}")]
    public async Task<ActionResult> GetMySubscriptionAsync(int userId)
    {
        if (userId < 1)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Id is not valid");
        }

        var result = await _subscriptionService.GetCurrentUserSubscriptionAsync(userId);

        result
            .OnSuccess(() =>
            {
                Log(LogLevel.Information, SubscriptionControllerEventIds.GetMySubscription,
                    "Retrieved current subscription for user {userId}.", userId);
            })
            .OnFailure(() =>
            {
                Log(LogLevel.Error, SubscriptionControllerEventIds.GetMySubscription,
                    "Error retrieving current subscription for user {userId}. Error: {error}",
                    userId, result.Error);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status500InternalServerError, result.Error)
            : StatusCode(StatusCodes.Status200OK, result.Value);
    }

    // [Authorize(Roles = "User")]
    [HttpPut("{subscriptionId:int}/change")]
    public async Task<ActionResult> ChangeSubscriptionAsync(int subscriptionId, int newPlanId)
    {
        if (subscriptionId < 1 || newPlanId < 1)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "At least one of ids is not valid");
        }

        var result = await _subscriptionService.ChangeSubscriptionAsync(subscriptionId, newPlanId);

        result
            .OnSuccess(() =>
            {
                Log(LogLevel.Information, SubscriptionControllerEventIds.ChangeSubscription,
                    "Changed subscription {subscriptionId} to plan {newPlanId}.", subscriptionId,
                    newPlanId);
            })
            .OnFailure(() =>
            {
                Log(LogLevel.Error, SubscriptionControllerEventIds.ChangeSubscription,
                    "Error changing subscription {subscriptionId} to plan {newPlanId}. Error: {error}",
                    subscriptionId, newPlanId, result.Error);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status400BadRequest, result.Error)
            : StatusCode(StatusCodes.Status204NoContent);
    }

    // [Authorize(Roles = "User")]
    [HttpPut("{userId:int}/{subscriptionId:int}/cancel")]
    public async Task<ActionResult> CancelSubscriptionAsync(int userId, int subscriptionId)
    {
        if (subscriptionId < 1 || userId < 1)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "At least one of ids is not valid");
        }
        var result = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);

        result
            .OnSuccess(() =>
            {
                Log(LogLevel.Information, SubscriptionControllerEventIds.CancelSubscription,
                    "Cancelled subscription {subscriptionId} for user {userId}.",
                    subscriptionId, userId);
            })
            .OnFailure(() =>
            {
                Log(LogLevel.Error, SubscriptionControllerEventIds.CancelSubscription,
                    "Error cancelling subscription {subscriptionId} for user {userId}. Error: {error}",
                    subscriptionId, userId, result.Error);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status400BadRequest, result.Error)
            : StatusCode(StatusCodes.Status204NoContent);
    }
}
