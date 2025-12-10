using System.Security.Claims;
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

    [Authorize(Roles = "Owner")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Creates a new subscription plan.")]
    [EndpointDescription("Validates the incoming plan payload and persists it as a subscription plan definition.")]
    public async Task<ActionResult> CreateSubscriptionPlanAsync([FromBody] CreateSubscriptionPlanDto subscriptionDto)
    {
        var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(ownerId))
        {
            Log(LogLevel.Warning, AuthControllerEventIds.TokenVerificationNoUserId, "User ID not found in claims");

            return Unauthorized();
        }

        var isParsed = int.TryParse(ownerId, out var parsedOwnerId);
        if (isParsed)
        {
            var result = await _subscriptionPlanService.CreateSubscriptionPlanAsync(parsedOwnerId, subscriptionDto);
            if (result.Failure)
            {
                return StatusCode(StatusCodes.Status400BadRequest, result.Error);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Lists all subscription plans.")]
    [EndpointDescription("Retrieves every subscription plan.")]
    public async Task<IActionResult> GetPlansAsync()
    {
        var result = await _subscriptionPlanService.GetPlansAsync();
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }
    
        return StatusCode(StatusCodes.Status200OK, result.Value);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Gets a subscription plan by id.")]
    [EndpointDescription("Fetches the plan details for the provided identifier or returns an error if unavailable.")]
    public async Task<IActionResult> GetPlanByIdAsync(int id)
    {
        var result = await _subscriptionPlanService.GetPlanByIdAsync(id);
        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status400BadRequest, result.Error);
        }
    
        return StatusCode(StatusCodes.Status200OK, result.Value);
    }

    [Authorize(Roles = "Owner")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Updates an existing subscription plan.")]
    [EndpointDescription("Logs the update attempt, validates payload data, and updates the specified subscription plan.")]
    public async Task<ActionResult> UpdateSubscriptionPlanAsync(int id, [FromBody] UpdateSubscriptionPlanDto subscriptionPlanDto)
    {
        if (id < 1 || subscriptionPlanDto is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Invalid request data");
        }

        var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(ownerId))
        {
            Log(LogLevel.Warning, AuthControllerEventIds.TokenVerificationNoUserId, "User ID not found in claims");

            return Unauthorized();
        }

        var isParsed = int.TryParse(ownerId, out var parsedOwnerId);
        if (isParsed)
        {
            var infoStartUpdating = LoggerMessage.Define<int>(
                LogLevel.Information,
                SubscriptionPlanControllerEventIds.UpdateSubscriptionPlanEvent,
                "Updating subscription plan with id {subscriptionPlanId}.");
            infoStartUpdating(Logger, id, null);

            var result = await _subscriptionPlanService.UpdateSubscriptionPlanAsync(
                id, subscriptionPlanDto, parsedOwnerId);

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

        return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpDelete("{subscriptionPlanId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Deletes a subscription plan.")]
    [EndpointDescription("Validates the identifier, logs the outcome, and removes the subscription plan if possible.")]
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

        var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(ownerId))
        {
            Log(LogLevel.Warning, AuthControllerEventIds.TokenVerificationNoUserId, "User ID not found in claims");

            return Unauthorized();
        }

        var isParsed = int.TryParse(ownerId, out _);
        if (isParsed)
        {
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

        return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [Authorize(Roles = "Owner, Admin")]
    [HttpPut("deactivate/{subscriptionPlanId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Deactivates a subscription plan.")]
    [EndpointDescription("Attempts to deactivate the specified plan while logging success or failure details.")]
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

        var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(ownerId))
        {
            Log(LogLevel.Warning, AuthControllerEventIds.TokenVerificationNoUserId, "User ID not found in claims");

            return Unauthorized();
        }

        var isParsed = int.TryParse(ownerId, out _);
        if (isParsed)
        {
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

        return StatusCode(StatusCodes.Status401Unauthorized);
    }
}
