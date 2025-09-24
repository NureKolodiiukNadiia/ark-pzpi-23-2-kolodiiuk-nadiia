using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoWorkingAccess.Infrastructure;

public static class DataSeed
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        Task.Run(async () =>
        {
            using var scope = serviceProvider.CreateScope();
            var provider = scope.ServiceProvider;
            var context = provider.GetRequiredService<CoWorkingAccessDbContext>();
            var userManager = provider.GetRequiredService<UserManager<User>>();

            // Subscription plans
            if (!await context.SubscriptionPlans.AnyAsync())
            {
                var plans = new[]
                {
                    new SubscriptionPlan
                    {
                        Name = "Basic", Description = "Basic plan", Price = 99m, Duration = Duration.Monthly,
                        IncludedHours = 40, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new SubscriptionPlan
                    {
                        Name = "Pro", Description = "Pro plan", Price = 199m, Duration = Duration.Monthly,
                        IncludedHours = 100, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new SubscriptionPlan
                    {
                        Name = "Daily", Description = "Daily plan", Price = 15m, Duration = Duration.Daily,
                        IncludedHours = 8, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    }
                };
                context.SubscriptionPlans.AddRange(plans);
                await context.SaveChangesAsync();
            }

            // Spaces
            if (!await context.Spaces.AnyAsync())
            {
                var spaces = new[]
                {
                    new Space
                    {
                        Name = "Conference Room", Description = "Large conference room", Type = SpaceType.MeetingRoom,
                        Capacity = 10, HourlyRate = 50m, IsAvailable = true, CreatedAt = DateTime.UtcNow
                    },
                    new Space
                    {
                        Name = "Open Desk", Description = "Open workspace area", Type = SpaceType.Desk, Capacity = 20,
                        HourlyRate = 20m, IsAvailable = true, CreatedAt = DateTime.UtcNow
                    },
                    new Space
                    {
                        Name = "Private Office", Description = "Private office for teams",
                        Type = SpaceType.PrivateOffice, Capacity = 5, HourlyRate = 80m, IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };
                context.Spaces.AddRange(spaces);
                await context.SaveChangesAsync();
            }

            // Devices
            if (!await context.Devices.AnyAsync())
            {
                var spaces = await context.Spaces.Take(3).ToListAsync();
                var devices = new[]
                {
                    new Device
                    {
                        SpaceId = spaces[0].Id, DeviceId = "LOCK-001", DeviceName = "Main Door Lock", IsOnline = true,
                        InstalledAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new Device
                    {
                        SpaceId = spaces[1].Id, DeviceId = "LOCK-002", DeviceName = "Desk Area Lock", IsOnline = true,
                        InstalledAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new Device
                    {
                        SpaceId = spaces[2].Id, DeviceId = "LOCK-003", DeviceName = "Office Lock", IsOnline = false,
                        InstalledAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    }
                };
                context.Devices.AddRange(devices);
                await context.SaveChangesAsync();
            }

            // Users (Identity)
            if (!await userManager.Users.AnyAsync())
            {
                var users = new[]
                {
                    new User
                    {
                        UserName = "admin", Email = "admin@cowork.com", PhoneNumber = "+10000000001",
                        FirstName = "Admin", LastName = "User", Role = Role.Admin, EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        UserName = "user1", Email = "user1@cowork.com", PhoneNumber = "+10000000002",
                        FirstName = "John", LastName = "Doe", Role = Role.User, EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        UserName = "user2", Email = "user2@cowork.com", PhoneNumber = "+10000000003",
                        FirstName = "Jane", LastName = "Smith", Role = Role.User, EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        UserName = "user3", Email = "user3@cowork.com", PhoneNumber = "+10000000004",
                        FirstName = "Alex", LastName = "Johnson", Role = Role.User, EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    }
                };
                await userManager.CreateAsync(users[0], "Admin123!");
                await userManager.CreateAsync(users[1], "User123!");
                await userManager.CreateAsync(users[2], "User123!");
                await userManager.CreateAsync(users[3], "User123!");
            }

            context.ChangeTracker.Clear();

            // Subscriptions
            if (!await context.Subscriptions.AnyAsync())
            {
                var users = await context.Users.Where(u => u.UserName != "admin").Take(3).ToListAsync();
                var plans = await context.SubscriptionPlans.Take(3).ToListAsync();
                var subscriptions = new[]
                {
                    new Subscription
                    {
                        UserId = users[0].Id, SubscriptionPlanId = plans[0].Id, Price = plans[0].Price,
                        StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1),
                        Status = SubscriptionStatus.Active, HoursUsed = 0, CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Subscription
                    {
                        UserId = users[1].Id, SubscriptionPlanId = plans[1].Id, Price = plans[1].Price,
                        StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1),
                        Status = SubscriptionStatus.Active, HoursUsed = 0, CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Subscription
                    {
                        UserId = users[2].Id, SubscriptionPlanId = plans[2].Id, Price = plans[2].Price,
                        StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1),
                        Status = SubscriptionStatus.Active, HoursUsed = 0, CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };
                context.Subscriptions.AddRange(subscriptions);
                await context.SaveChangesAsync();
            }

            // Bookings
            if (!await context.Bookings.AnyAsync())
            {
                var users = await context.Users.Where(u => u.UserName != "admin").Take(3).ToListAsync();
                var spaces = await context.Spaces.Take(3).ToListAsync();
                var bookings = new[]
                {
                    new Booking
                    {
                        UserId = users[0].Id, SpaceId = spaces[0].Id,
                        StartTime = DateTime.UtcNow.AddDays(1).AddHours(9),
                        EndTime = DateTime.UtcNow.AddDays(1).AddHours(11), Status = BookingStatus.Confirmed,
                        TotalAmount = spaces[0].HourlyRate * 2, BookingType = Duration.Hourly,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new Booking
                    {
                        UserId = users[1].Id, SpaceId = spaces[1].Id,
                        StartTime = DateTime.UtcNow.AddDays(2).AddHours(10),
                        EndTime = DateTime.UtcNow.AddDays(2).AddHours(12), Status = BookingStatus.Confirmed,
                        TotalAmount = spaces[1].HourlyRate * 2, BookingType = Duration.Hourly,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    },
                    new Booking
                    {
                        UserId = users[2].Id, SpaceId = spaces[2].Id,
                        StartTime = DateTime.UtcNow.AddDays(3).AddHours(8),
                        EndTime = DateTime.UtcNow.AddDays(3).AddHours(10), Status = BookingStatus.Confirmed,
                        TotalAmount = spaces[2].HourlyRate * 2, BookingType = Duration.Hourly,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    }
                };
                context.Bookings.AddRange(bookings);
                await context.SaveChangesAsync();
            }

            // Payments
            if (!await context.Payments.AnyAsync())
            {
                var users = await context.Users.Where(u => u.UserName != "admin").Take(3).ToListAsync();
                var bookings = await context.Bookings.Take(3).ToListAsync();
                var payments = new[]
                {
                    new Payment
                    {
                        UserId = users[0].Id, BookingId = bookings[0].Id, Amount = bookings[0].TotalAmount,
                        Status = PaymentStatus.Paid, TransactionId = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow, ProcessedAt = DateTime.UtcNow
                    },
                    new Payment
                    {
                        UserId = users[1].Id, BookingId = bookings[1].Id, Amount = bookings[1].TotalAmount,
                        Status = PaymentStatus.Paid, TransactionId = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow, ProcessedAt = DateTime.UtcNow
                    },
                    new Payment
                    {
                        UserId = users[2].Id, BookingId = bookings[2].Id, Amount = bookings[2].TotalAmount,
                        Status = PaymentStatus.Paid, TransactionId = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow, ProcessedAt = DateTime.UtcNow
                    }
                };
                context.Payments.AddRange(payments);
                await context.SaveChangesAsync();
            }

            // AccessLogs
            if (!await context.AccessLogs.AnyAsync())
            {
                var users = await context.Users.Where(u => u.UserName != "admin").Take(3).ToListAsync();
                var devices = await context.Devices.Take(3).ToListAsync();
                var logs = new[]
                {
                    new AccessLog
                    {
                        UserId = users[0].Id, DeviceId = devices[0].Id, SpaceId = devices[0].SpaceId,
                        AccessType = AccessType.Entry, Timestamp = DateTime.UtcNow, IsSuccessful = true
                    },
                    new AccessLog
                    {
                        UserId = users[1].Id, DeviceId = devices[1].Id, SpaceId = devices[1].SpaceId,
                        AccessType = AccessType.Exit, Timestamp = DateTime.UtcNow.AddMinutes(30), IsSuccessful = true
                    },
                    new AccessLog
                    {
                        UserId = users[2].Id, DeviceId = devices[2].Id, SpaceId = devices[2].SpaceId,
                        AccessType = AccessType.Entry, Timestamp = DateTime.UtcNow.AddMinutes(60), IsSuccessful = false,
                        ErrorMessage = "Access denied"
                    }
                };
                context.AccessLogs.AddRange(logs);
                await context.SaveChangesAsync();
            }
        }).GetAwaiter().GetResult();
    }
}