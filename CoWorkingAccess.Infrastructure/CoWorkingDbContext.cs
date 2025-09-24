using System.Reflection;
using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoWorkingAccess.Infrastructure;

public class CoWorkingAccessDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public CoWorkingAccessDbContext(DbContextOptions<CoWorkingAccessDbContext> options) : base(options)
    {
    }

    public DbSet<AccessLog> AccessLogs { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Space> Spaces { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // builder.UseSnakeCaseNamingConvention();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(CoWorkingAccessDbContext)));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=atark;Username=myuser;Password=mypassword;");
    }
}
