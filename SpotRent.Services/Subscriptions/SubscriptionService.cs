using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Domain.Extensions;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Logging;
using SpotRent.Services.Payment;

namespace SpotRent.Services.Subscriptions;

public class SubscriptionService : BaseService<SubscriptionService>, ISubscriptionService
{
    private readonly IPaymentService _paymentService;

    public SubscriptionService(
        SpotRentDbContext context,
        ILogger<SubscriptionService> logger,
        IPaymentService paymentService)
        : base(context, logger)
    {
        _paymentService = paymentService;
    }

    public async Task<Result<SubscriptionCreationResponse>> SubscribeAsync(int userId, int subscriptionPlanId)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        SubscriptionPlan subscriptionPlan;
        try
        {
            var user = await Context.Users.FindAsync(userId);
            if (user == null)
            {
                return Result.Fail<SubscriptionCreationResponse>($"No user {userId} specified in order request");
            }

            var existingSubscription = await Context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
            if (existingSubscription is not null)
            {
                return Result.Fail<SubscriptionCreationResponse>(
                    $"User {userId} is already subscribed. Update subscription instead");
            }

            subscriptionPlan = await Context.SubscriptionPlans.FindAsync(subscriptionPlanId);
            if (subscriptionPlan is null || !subscriptionPlan.IsActive)
            {
                return Result.Fail<SubscriptionCreationResponse>(
                    $"Subscription plan with id {subscriptionPlanId} not available");
            }

            var subscription = await CreateSubscription();
            if (subscription is null)
            {
                return Result.Fail<SubscriptionCreationResponse>("Invalid duration");
            }

            var paymentDataResult = await _paymentService.CreatePaymentAsync(subscription.Id, subscription.TotalAmount);
            if (paymentDataResult.Failure)
            {
                return Result.Fail<SubscriptionCreationResponse>($"{paymentDataResult.Error}");
            }

            scope.Complete();

            return Result.Success(new SubscriptionCreationResponse
                { SubscriptionId = subscription.Id, LiqPayPaymentData = paymentDataResult.Value });
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.Subscribe,
                "DB error subscribing user {userId} to plan {planId}. Error: {error}",
                userId, subscriptionPlanId, e.Message);

            return Result.Fail<SubscriptionCreationResponse>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.Subscribe,
                "Error subscribing user {userId} to plan {planId}. Error: {error}",
                userId, subscriptionPlanId, e.Message);

            return Result.Fail<SubscriptionCreationResponse>($"Failure placing order: {e.Message}");
        }

        async Task<Subscription> CreateSubscription()
        {
            var now = DateTime.UtcNow;
            var start = now;
            var end = CalcEndDate(start, subscriptionPlan.Duration);
            if (end is null)
            {
                return null;
            }

            var subscription = new Subscription
            {
                UserId = userId,
                SubscriptionPlanId = subscriptionPlan.Id,
                Price = subscriptionPlan.Price,
                StartDate = start,
                EndDate = end.Value,
                Status = SubscriptionStatus.NotPaid,
                HoursUsed = 0,
                TotalAmount = subscriptionPlan.Price,
                PaymentStatus = PaymentStatus.NotPaid,
                TransactionId = 0, //todo: make nullable
                CreatedAt = now,
                UpdatedAt = now,
                CancelledAt = null
            };

            await Context.AddAsync(subscription);
            await Context.SaveChangesAsync();

            return subscription;
        }
    }

    public async Task<Result<SubscriptionDto>> GetCurrentUserSubscriptionAsync(int userId)
    {
        try
        {
            var user = await Context.Users.FindAsync(userId);
            if (user is null)
            {
                return Result.Fail<SubscriptionDto>($"No user with id: {userId}");
            }

            var subscription = await Context.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId);
            if (subscription is null)
            {
                return Result.Fail<SubscriptionDto>($"User with id {userId} is not subscribed");
            }

            var dto = new SubscriptionDto
            {
                Id = subscription.Id,
                SubscriptionPlanId = subscription.SubscriptionPlanId,
                Price = subscription.Price,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                Status = subscription.Status,
                HoursUsed = subscription.HoursUsed,
                TotalAmount = subscription.TotalAmount,
                PaymentStatus = subscription.PaymentStatus,
                PaymentProcessedAt = subscription.PaymentProcessedAt,
                PaymentFailureReason = subscription.PaymentFailureReason,
                CreatedAt = subscription.CreatedAt,
                UpdatedAt = subscription.UpdatedAt,
            };

            return Result.Success(dto);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.GetCurrentUserSubscription,
                "DB error retrieving current subscription for user {userId}. Error: {error}",
                userId, e.Message);

            return Result.Fail<SubscriptionDto>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.GetCurrentUserSubscription,
                "Error retrieving current subscription for user {userId}. Error: {error}",
                userId, e.Message);

            return Result.Fail<SubscriptionDto>($"Failure retrieving user subscription: {e.Message}.");
        }
    }

    public async Task<Result<IEnumerable<SubscriptionInfo>>> GetSubscriptionHistoryAsync(int userId)
    {
        try
        {
            var history = await Context.Subscriptions
                .Include(s => s.SubscriptionPlan)
                .Where(s => s.UserId == userId)
                .Select(s => new SubscriptionInfo
                {
                    Id = s.Id,
                    SubscriptionPlanId = s.SubscriptionPlanId,
                    SubscriptionPlanName = s.SubscriptionPlan.Name,
                    SubscriptionStatus = s.Status,
                    IsActive = s.IsActive(),
                    StartedAt = s.StartDate,
                    ExpiresAt = s.EndDate,
                })
                .ToListAsync();

            return Result.Success<IEnumerable<SubscriptionInfo>>(history);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.GetSubscriptionHistory,
                "DB error retrieving subscription history for user {userId}. Error: {error}",
                userId, e.Message);

            return Result.Fail<IEnumerable<SubscriptionInfo>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.GetSubscriptionHistory,
                "Error retrieving subscription history for user {userId}. Error: {error}",
                userId, e.Message);

            return Result.Fail<IEnumerable<SubscriptionInfo>>(
                $"Failure retrieving subscription plans: {e.Message}.");
        }
    }

    public async Task<Result<Subscription>> GetSubscriptionByIdAsync(int id)
    {
        try
        {
            var subscription = await Context.Subscriptions.FindAsync(id);

            return subscription is null
                ? Result.Fail<Subscription>($"No subscription plan with id: {id}")
                : Result.Success(subscription);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.GetSubscriptionById,
                "DB error retrieving subscription with id {id}. Error: {error}",
                id, e.Message);

            return Result.Fail<Subscription>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.GetSubscriptionById,
                "Error retrieving subscription with id {id}. Error: {error}",
                id, e.Message);

            return Result.Fail<Subscription>($"Failure retrieving subscription with id {id}: {e.Message}.");
        }
    }

    public async Task<Result> ChangeSubscriptionAsync(int currSubscriptionId, int newPlanId, int userId)
    {
        try
        {
            var user = await Context.Users.FindAsync(userId);
            if (user is null)
            {
                return Result.Fail($"No user with id {userId}");
            }

            var subscription = await Context.Subscriptions.FindAsync(currSubscriptionId);
            if (subscription is null || subscription.UserId != userId)
            {
                return Result.Fail($"No subscription with id {currSubscriptionId}");
            }

            subscription.SubscriptionPlanId = newPlanId;
            Context.Update(subscription);
            await Context.SaveChangesAsync();

            return Result.Success();
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.ChangeSubscription,
                "DB error changing subscription {subscriptionId} to plan {newPlanId}. Error: {error}",
                currSubscriptionId, newPlanId, e.Message);

            return Result.Fail($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SubscriptionServiceEventIds.ChangeSubscription,
                "Error changing subscription {subscriptionId} to plan {newPlanId}. Error: {error}",
                currSubscriptionId, newPlanId, e.Message);

            return Result.Fail($"Failure changing subscription plan: {e.Message}");
        }
    }

    public async Task<Result> CancelSubscriptionAsync(int subscriptionId, int userId)
    {
        try
        {
            var user = await Context.Users.FindAsync(userId);
            if (user is null)
            {
                return Result.Fail($"No user with id {userId}");
            }

            var subscription = await Context.Subscriptions.FindAsync(subscriptionId);
            if (subscription is null || subscription.UserId != userId)
            {
                return Result.Fail<LiqPayRefundResponse>(
                    $"No subscription with id {subscriptionId} of the user {userId}");
            }

            var stateTransitionNotValid = subscription.Status ==
                                          (SubscriptionStatus.Cancelled | SubscriptionStatus.Expired);
            if (stateTransitionNotValid)
            {
                return Result.Fail<LiqPayRefundResponse>($"Subscription with id {subscriptionId} can't be cancelled");
            }

            var result = await _paymentService.RefundPaymentAsync(subscriptionId);
            result.OnSuccess(() => Serilog.Log.Information("Success refunding payment"))
                .OnFailure(() => Serilog.Log.Error(result.Error));

            if (result.Value.Result == "error")
            {
                return Result.Fail<LiqPayRefundResponse>($"Error refunding: {result.Value.Status}");
            }

            subscription.Status = SubscriptionStatus.Cancelled;
            Context.Subscriptions.Update(subscription);
            await Context.SaveChangesAsync();

            return result.Failure
                ? Result.Fail<LiqPayRefundResponse>($"Payment refund failed. Reason: {result.Error}")
                : Result.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result.Fail<LiqPayRefundResponse>($"Error refunding: {e.Message}");
        }
    }

    private DateTime? CalcEndDate(DateTime startDate, Duration duration)
    {
        double durationInDays;
        switch (duration)
        {
            case Duration.Week:
                durationInDays = 7;
                break;
            case Duration.TwoWeeks:
                durationInDays = 14;
                break;
            case Duration.Month:
                durationInDays = 30;
                break;
            case Duration.ThreeMonths:
                durationInDays = 90;
                break;
            default:
                return null;
        }

        return startDate.AddDays(durationInDays);
    }
}
