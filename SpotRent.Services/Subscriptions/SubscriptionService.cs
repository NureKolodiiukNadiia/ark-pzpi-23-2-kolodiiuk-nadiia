using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;
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

    public async Task<Result<LiqPayPaymentData>> SubscribeAsync(int userId, int subscriptionPlanId)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            var user = await Context.Users.FindAsync(userId);
            if (user == null)
            {
                return Result.Fail<LiqPayPaymentData>($"No user {userId} specified in order request");
            }

            var subscriptionPlan = await Context.SubscriptionPlans.FindAsync(subscriptionPlanId);
            if (subscriptionPlan is null || !subscriptionPlan.IsActive)
            {
                return Result.Fail<LiqPayPaymentData>($"Subscription plan with id {subscriptionPlanId} not available");
            }

            var subscription = new Subscription
            {
                Status = SubscriptionStatus.NotPaid,
                TotalAmount = subscriptionPlan.Price,
                PaymentStatus = PaymentStatus.NotPaid,

                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await Context.AddAsync(subscription);
            await Context.SaveChangesAsync();

            var paymentDataResult = await _paymentService.CreatePayment(subscription.Id, subscription.TotalAmount);
            if (paymentDataResult.Failure)
            {
                return Result.Fail<LiqPayPaymentData>($"{paymentDataResult.Error}");
            }

            scope.Complete();

            return Result.Success(paymentDataResult.Value);
        }
        catch (NpgsqlException e)
        {
            return Result.Fail<LiqPayPaymentData>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail<LiqPayPaymentData>($"Failure placing order: {e.Message}");
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
            return Result.Fail<SubscriptionDto>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail<SubscriptionDto>($"Failure retrieving user subscription: {e.Message}.");
        }
    }

    public async Task<Result<SubscriptionHistory>> GetSubscriptionHistoryAsync(int userId, int page, int pageSize)
    {
        try
        {
            var plans = await Context.SubscriptionPlans
                .Where(sp => sp.IsActive == true)
                .Select(sp => new SubscriptionPlanDto
                {
                    Id = sp.Id,
                    Name = sp.Name,
                    Description = sp.Description,
                    Price = sp.Price,
                    Duration = sp.Duration,
                    IncludedHours = sp.IncludedHours,
                })
                .ToListAsync();

            return Result.Success<IEnumerable<SubscriptionPlanDto>>(plans);
        }
        catch (NpgsqlException e)
        {
            return Result.Fail<IEnumerable<SubscriptionPlanDto>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<SubscriptionPlanDto>>(
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
            return Result.Fail<Subscription>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail<Subscription>($"Failure retrieving subscription with id {id}: {e.Message}.");
        }
    }

    public async Task<Result<Subscription>> ChangeSubscriptionAsync(int currSubscriptionId, int newPlanId)
    {
        try
        {
            var user = await Context.Users.FindAsync(userId);
            if (user == null)
            {
                return Result.Fail<LiqPayPaymentData>($"No user {userId} specified in order request");
            }

            var subscriptionPlan = await Context.SubscriptionPlans.FindAsync(subscriptionPlanId);
            if (subscriptionPlan is null || !subscriptionPlan.IsActive)
            {
                return Result.Fail<LiqPayPaymentData>($"Subscription plan with id {subscriptionPlanId} not available");
            }

            var subscription = new Subscription
            {
                Status = SubscriptionStatus.NotPaid,
                TotalAmount = subscriptionPlan.Price,
                PaymentStatus = PaymentStatus.NotPaid,

                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await Context.AddAsync(subscription);
            await Context.SaveChangesAsync();

            var paymentDataResult = await _paymentService.CreatePayment(subscription.Id, subscription.TotalAmount);
            if (paymentDataResult.Failure)
            {
                return Result.Fail<LiqPayPaymentData>($"{paymentDataResult.Error}");
            }

            scope.Complete();

            return Result.Success(paymentDataResult.Value);
        }
        catch (NpgsqlException e)
        {
            return Result.Fail<LiqPayPaymentData>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail<LiqPayPaymentData>($"Failure placing order: {e.Message}");
        }
    }

    public async Task<Result> CancelSubscriptionAsync(int subscriptionId)
    {
        throw new NotImplementedException();
        var subscription = await Context.Subscriptions.FindAsync(subscriptionId);
        if (subscription == null)
        {
            return Result.Fail("No subscription with specified id");
        }

        try
        {
            Context.Remove(subscription);
            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Fail($"{e.Message}");
        }

        return Result.Success();
    }
}
