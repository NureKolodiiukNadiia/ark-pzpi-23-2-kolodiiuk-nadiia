using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Subscriptions;

public class SubscriptionPlanService : BaseService<SubscriptionPlanService>, ISubscriptionPlanService
{
    public SubscriptionPlanService(SpotRentDbContext context, ILogger<SubscriptionPlanService> logger) : base(context,
        logger)
    {
    }

    public async Task<Result<IEnumerable<SubscriptionPlanDto>>> GetPlansAsync()
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


    public async Task<Result<SubscriptionPlanDto>> GetPlanByIdAsync(int id)
    {
        try
        {
            var plan = await Context.SubscriptionPlans.FindAsync(id);
            if (plan is null)
            {
                return Result.Fail<SubscriptionPlanDto>($"No subscription plan with id: {id}");
            }

            var planDto = new SubscriptionPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                Duration = plan.Duration,
                IncludedHours = plan.IncludedHours,
            };

            return Result.Success(planDto);
        }
        catch (NpgsqlException e)
        {
            return Result.Fail<SubscriptionPlanDto>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail<SubscriptionPlanDto>(
                $"Failure retrieving subscription plan: {e.Message}.");
        }
    }

    public async Task<Result> CreateSubscriptionPlanAsync(CreateSubscriptionPlanDto subscriptionPlanDto)
    {
        try
        {
            var plan = new SubscriptionPlan
            {
                Name = subscriptionPlanDto.Name,
                Description = subscriptionPlanDto.Description,
                Price = subscriptionPlanDto.Price,
                Duration = subscriptionPlanDto.Duration,
                IncludedHours = subscriptionPlanDto.IncludedHours,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await Context.AddAsync(plan);
            await Context.SaveChangesAsync();

            return Result.Success();
        }
        catch (NpgsqlException e)
        {
            return Result.Fail($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail($"Failure creating subscription plan: {e.Message}.");
        }
    }

    public async Task<Result> UpdateSubscriptionPlanAsync(UpdateSubscriptionPlanDto subscriptionPlanDto)
    {
        try
        {
            var plan = await Context.SubscriptionPlans.FindAsync(subscriptionPlanDto.Id);
            if (plan is null)
            {
                return Result.Fail($"No subscription plan with id: {subscriptionPlanDto.Id}");
            }

            plan.UpdatedAt = DateTime.UtcNow;
            plan.Name = subscriptionPlanDto.Name;
            plan.Description = subscriptionPlanDto.Description;
            plan.Price = subscriptionPlanDto.Price;

            Context.Update(plan);
            await Context.SaveChangesAsync();

            return Result.Success();
        }
        catch (NpgsqlException e)
        {
            return Result.Fail($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail($"Failure updating subscription plan: {e.Message}.");
        }
    }

    public async Task<Result> DeactivateSubscriptionPlanAsync(int subscriptionPlanId)
    {
        try
        {
            var plan = await Context.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == subscriptionPlanId);
            if (plan is null)
            {
                return Result.Fail($"No subscription plan with id: {subscriptionPlanId}");
            }

            if (!plan.IsActive)
            {
                return Result.Success();
            }

            // when deactivated plan can be got by id, but not with GetPlans
            plan.IsActive = false;
            Context.Update(plan);
            await Context.SaveChangesAsync();

            return Result.Success();
        }
        catch (NpgsqlException e)
        {
            return Result.Fail($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail($"Failure deleting subscription plan: {e.Message}.");
        }
    }

    public async Task<Result> DeleteSubscriptionPlanAsync(int subscriptionPlanId)
    {
        try
        {
            var plan = await Context.SubscriptionPlans
                .Include(p => p.Subscriptions)
                .FirstOrDefaultAsync(p => p.Id == subscriptionPlanId);
            if (plan is null)
            {
                return Result.Fail($"No subscription plan with id: {subscriptionPlanId}");
            }

            if (plan.IsActive || plan.Subscriptions.Count != 0)
            {
                return Result.Fail($"There are active subscriptions on subscription {subscriptionPlanId}.");
            }

            Context.Remove(plan);
            await Context.SaveChangesAsync();

            return Result.Success();
        }
        catch (NpgsqlException e)
        {
            return Result.Fail($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            return Result.Fail($"Failure deleting subscription plan: {e.Message}.");
        }
    }
}
