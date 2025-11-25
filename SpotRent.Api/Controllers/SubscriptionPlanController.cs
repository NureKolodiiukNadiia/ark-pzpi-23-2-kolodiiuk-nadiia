using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Api.Logging;
using SpotRent.Domain.Extensions;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Subscriptions;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionPlanController : BaseController<SubscriptionPlanController>
{
    private readonly ISubscriptionPlanService _subscriptionPlanService;

    public SubscriptionPlanController(
        ISubscriptionPlanService subscriptionPlanService,
        ILogger<SubscriptionPlanController> logger)
    : base(logger)
    {
        _subscriptionPlanService = subscriptionPlanService;
    }

    // [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateSubscriptionPlanAsync([FromBody] CreateSubscriptionPlanDto subscriptionDto)
    {
        var result = await _subscriptionPlanService.CreateSubscriptionPlanAsync(subscriptionDto);
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    public async Task<IActionResult> GetPlansAsync()
    {
        var result = await _subscriptionPlanService.GetPlansAsync();
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }
    
        return StatusCode(StatusCodes.Status200OK, new { data = result.Value });
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPlanByIdAsync(int id)
    {
        var result = await _subscriptionPlanService.GetPlanByIdAsync(id);
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }
    
        return StatusCode(StatusCodes.Status200OK, result.Value);
    }

    // [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateSubscriptionPlanAsync(int id, [FromBody] UpdateSubscriptionPlanDto subscriptionPlanDto)
    {
        if (id < 1 || subscriptionPlanDto is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Invalid request data");
        }

        var infoStartUpdating = LoggerMessage.Define<int>(
            LogLevel.Information,
            SubscriptionPlanControllerEventIds.UpdateSubscriptionPlanEvent,
            "Updating subscription plan with id {subscriptionPlanId}.");
        infoStartUpdating(Logger, id, null);

        var result = await _subscriptionPlanService.UpdateSubscriptionPlanAsync(id, subscriptionPlanDto);

        result
            .OnSuccess(() =>
            {
                var infoUpdated = LoggerMessage.Define<int>(
                    LogLevel.Information,
                    SubscriptionPlanControllerEventIds.UpdateSubscriptionPlanEvent,
                    "Updated subscription plan with id {subscriptionPlanId}.");
                infoUpdated(Logger, id, null);
            })
            .OnFailure(() =>
            {
                var failureUpdating = LoggerMessage.Define<int, string>(
                    LogLevel.Error,
                    SubscriptionPlanControllerEventIds.UpdateSubscriptionPlanEvent,
                    "Error updating subscription plan with id {subscriptionPlanId}. Error: {error}");
                failureUpdating(Logger, id, result.Error, null);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status400BadRequest, result.Error)
            : StatusCode(StatusCodes.Status200OK);
    }

    // [Authorize(Roles = "Admin")]
    [HttpDelete("{subscriptionPlanId:int}")]
    public async Task<IActionResult> DeleteSubscriptionPlanAsync(int subscriptionPlanId)
    {
        if (subscriptionPlanId < 1)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Id is less than 1");
        }

        var infoStartDeleting = LoggerMessage.Define<int>(
            LogLevel.Information,
            SubscriptionPlanControllerEventIds.DeleteSubscriptionPlanEvent,
            "Deleting subscription plan with id {subscriptionPlanId}.");
        infoStartDeleting(Logger, subscriptionPlanId, null);

        var result = await _subscriptionPlanService.DeleteSubscriptionPlanAsync(subscriptionPlanId);

        result
            .OnSuccess(() =>
            {
                var infoDeleted = LoggerMessage.Define<int>(
                    LogLevel.Information,
                    SubscriptionPlanControllerEventIds.DeleteSubscriptionPlanEvent,
                    "Deleted subscription plan with id {subscriptionPlanId}.");
                infoDeleted(Logger, subscriptionPlanId, null);
            })
            .OnFailure(() =>
            {
                var failureDeleting = LoggerMessage.Define<int, string>(
                    LogLevel.Error,
                    SubscriptionPlanControllerEventIds.DeleteSubscriptionPlanEvent,
                    "Error deleting subscription plan with id {subscriptionPlanId}. Error: {error}");
                failureDeleting(Logger, subscriptionPlanId, result.Error, null);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status400BadRequest, result.Error)
            : StatusCode(StatusCodes.Status200OK);
    }

    // [Authorize(Roles = "Admin")]
    [HttpPut("deactivate/{subscriptionPlanId:int}")]
    public async Task<IActionResult> DeactivateSubscriptionPlanAsync(int subscriptionPlanId)
    {
        if (subscriptionPlanId < 1)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Id is less than 1");
        }

        var infoStartDeactivating = LoggerMessage.Define<int>(
            LogLevel.Information,
            SubscriptionPlanControllerEventIds.DeactivateSubscriptionPlanEvent,
            "Deactivating subscription plan with id {subscriptionPlanId}.");
        infoStartDeactivating(Logger, subscriptionPlanId, null);

        var result = await _subscriptionPlanService.DeactivateSubscriptionPlanAsync(subscriptionPlanId);

        result
            .OnSuccess(() =>
            {
                var infoDeactivated = LoggerMessage.Define<int>(
                    LogLevel.Information,
                    SubscriptionPlanControllerEventIds.DeactivateSubscriptionPlanEvent,
                    "Deactivated subscription plan with id {subscriptionPlanId}.");
                infoDeactivated(Logger, subscriptionPlanId, null);
            })
            .OnFailure(() =>
            {
                var failureDeactivating = LoggerMessage.Define<int, string>(
                    LogLevel.Error,
                    SubscriptionPlanControllerEventIds.DeactivateSubscriptionPlanEvent,
                    "Error deactivating subscription plan with id {subscriptionPlanId}. Error: {error}");
                failureDeactivating(Logger, subscriptionPlanId, result.Error, null);
            });

        return result.Failure
            ? StatusCode(StatusCodes.Status400BadRequest, result.Error)
            : StatusCode(StatusCodes.Status200OK);
    }
}

