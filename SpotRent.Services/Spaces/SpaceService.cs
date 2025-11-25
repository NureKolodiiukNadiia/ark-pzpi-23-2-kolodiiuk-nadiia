using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Logging;

namespace SpotRent.Services.Spaces;

public class SpaceService : BaseService<SpaceService>, ISpaceService
{
    public SpaceService(SpotRentDbContext context, ILogger<SpaceService> logger) : base(context, logger)
    {
    }

    public async Task<Result<Space>> CreateSpaceAsync(Space space)
    {
        if (space is null)
        {
            return Result.Fail<Space>("Space payload cannot be null");
        }

        try
        {
            var now = DateTime.UtcNow;
            space.CreatedAt = now;
            space.UpdatedAt = now;

            if (space.AttributeValues != null)
            {
                foreach (var av in space.AttributeValues)
                {
                    av.Space = space;
                }
            }

            await Context.Spaces.AddAsync(space);
            await Context.SaveChangesAsync();

            return Result.Success(space);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.CreateSpace,
                "DB error creating space. Error: {error}", e.Message);

            return Result.Fail<Space>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.CreateSpace,
                "Error creating space. Error: {error}", e.Message);

            return Result.Fail<Space>($"Failure creating space: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<Space>>> FilterSpacesAsync(SpaceFilterRequest req)
    {
        try
        {
            var query = Context.Spaces.AsQueryable();

            query = req.Includes(query);
            query = query.AsExpandable().Where(req.Predicate);
            query = req.OrderBy(query);

            var total = await query.CountAsync();
            req.CaptureTotal?.Invoke(total);

            if (req.SkipCount > 0)
            {
                query = query.Skip(req.SkipCount);
            }

            if (req.TakeCount is > 0)
            {
                query = query.Take(req.TakeCount.Value);
            }

            var spaces = await query.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<Space>>(spaces);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.FilterSpaces,
                "DB error filtering spaces. Error: {error}", e.Message);

            return Result.Fail<IEnumerable<Space>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.FilterSpaces,
                "Error filtering spaces. Error: {error}", e.Message);

            return Result.Fail<IEnumerable<Space>>($"Failure filtering spaces: {e.Message}");
        }
    }

    public async Task<Result<Space>> GetSpaceByIdAsync(int id)
    {
        try
        {
            var space = await Context.Spaces
                .Include(s => s.Address)
                .Include(s => s.Owner)
                .Include(s => s.AttributeValues)
                .ThenInclude(av => av.Attribute)
                .Include(s => s.WorkingHours)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (space is null)
            {
                return Result.Fail<Space>($"No space with id: {id}");
            }

            return Result.Success(space);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.GetSpaceById,
                "DB error retrieving space {id}. Error: {error}", id, e.Message);

            return Result.Fail<Space>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.GetSpaceById,
                "Error retrieving space {id}. Error: {error}", id, e.Message);

            return Result.Fail<Space>($"Failure retrieving space: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<Space>>> GetAvailableSpacesAsync(
        DateTime startTime, DateTime endTime, string city)
    {
        if (endTime <= startTime)
        {
            return Result.Fail<IEnumerable<Space>>("End time must be after start time");
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            return Result.Fail<IEnumerable<Space>>("City must be provided");
        }

        try
        {
            var spaces = await Context.Spaces
                .Include(s => s.Address)
                .Include(s => s.Owner)
                .Include(s => s.WorkingHours)
                .Include(s => s.AttributeValues)
                .ThenInclude(av => av.Attribute)
                .Include(s => s.Bookings)
                .Where(s => s.Address != null && s.Address.City == city && s.IsAvailable)
                .Where(s => !s.Bookings.Any(b =>
                    b.CancelledAt == null &&
                    b.StartTime < endTime &&
                    b.EndTime > startTime))
                .AsNoTracking()
                .ToListAsync();

            return Result.Success<IEnumerable<Space>>(spaces);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.GetAvailableSpaces,
                "DB error retrieving available spaces. Error: {error}", e.Message);

            return Result.Fail<IEnumerable<Space>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.GetAvailableSpaces,
                "Error retrieving available spaces. Error: {error}", e.Message);

            return Result.Fail<IEnumerable<Space>>($"Failure retrieving spaces: {e.Message}");
        }
    }

    public async Task<Result<bool>> IsSpaceAvailableAsync(int spaceId, DateTime startTime, DateTime endTime)
    {
        if (spaceId < 1)
        {
            return Result.Fail<bool>("Space id must be positive");
        }

        if (endTime <= startTime)
        {
            return Result.Fail<bool>("End time must be after start time");
        }

        try
        {
            var overlaps = await Context.Bookings
                .Where(b => b.SpaceId == spaceId && b.CancelledAt == null)
                .AnyAsync(b => b.StartTime < endTime && b.EndTime > startTime);

            return Result.Success(!overlaps);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.CheckAvailability,
                "DB error checking availability {spaceId}. Error: {error}", spaceId, e.Message);

            return Result.Fail<bool>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.CheckAvailability,
                "Error checking availability {spaceId}. Error: {error}", spaceId, e.Message);

            return Result.Fail<bool>($"Failure checking availability: {e.Message}");
        }
    }

    public async Task<Result<SpaceSchedule>> GetSpaceScheduleAsync(int spaceId, DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var exists = await Context.Spaces.AnyAsync(s => s.Id == spaceId);
            if (!exists)
            {
                return Result.Fail<SpaceSchedule>($"No space with id: {spaceId}");
            }

            var bookings = await Context.Bookings
                .Where(b => b.SpaceId == spaceId && b.CancelledAt == null)
                .Where(b => b.StartTime < endDate && b.EndTime > startDate && endDate < b.EndTime && startDate > b.StartTime)
                .OrderBy(b => b.StartTime)
                .Select(b => new { b.StartTime, b.EndTime })
                .ToListAsync();

            var schedule = new SpaceSchedule
            {
                SpaceId = spaceId,
                Bookings = bookings.Select(b => new StartEndTime {StartTime = b.StartTime, EndTime = b.EndTime}).ToList()
            };

            return Result.Success(schedule);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.GetSpaceSchedule,
                "DB error retrieving schedule {spaceId}. Error: {error}", spaceId, e.Message);

            return Result.Fail<SpaceSchedule>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.GetSpaceSchedule,
                "Error retrieving schedule {spaceId}. Error: {error}", spaceId, e.Message);

            return Result.Fail<SpaceSchedule>($"Failure retrieving schedule: {e.Message}");
        }
    }

    public async Task<Result> UpdateSpaceAsync(Space space)
    {
        if (space is null || space.Id < 1)
        {
            return Result.Fail("Space payload is invalid");
        }

        try
        {
            var existingSpace = await Context.Spaces
                .Include(s => s.AttributeValues)
                .Include(s => s.WorkingHours)
                .FirstOrDefaultAsync(s => s.Id == space.Id);

            if (existingSpace is null)
            {
                return Result.Fail($"No space with id: {space.Id}");
            }

            // Update scalar properties
            Context.Entry(existingSpace).CurrentValues.SetValues(space);
            existingSpace.UpdatedAt = DateTime.UtcNow;

            // Sync AttributeValues
            var incomingAttr = space.AttributeValues ?? new List<AttributeValue>();
            // Remove missing
            foreach (var existingAv in existingSpace.AttributeValues.ToList())
            {
                if (!incomingAttr.Any(a => a.Id == existingAv.Id && a.Id > 0))
                {
                    Context.Remove(existingAv);
                }
            }

            // Add or update incoming
            foreach (var av in incomingAttr)
            {
                if (av.Id > 0)
                {
                    var match = existingSpace.AttributeValues.FirstOrDefault(a => a.Id == av.Id);
                    if (match != null)
                    {
                        Context.Entry(match).CurrentValues.SetValues(av);
                    }
                }
                else
                {
                    av.Space = existingSpace;
                    existingSpace.AttributeValues.Add(av);
                }
            }

            // Sync WorkingHours
            var incomingWh = space.WorkingHours ?? new List<WorkingHours>();
            foreach (var existingWh in existingSpace.WorkingHours.ToList())
            {
                if (!incomingWh.Any(w => w.Id == existingWh.Id && w.Id > 0))
                {
                    Context.Remove(existingWh);
                }
            }

            foreach (var wh in incomingWh)
            {
                if (wh.Id > 0)
                {
                    var match = existingSpace.WorkingHours.FirstOrDefault(w => w.Id == wh.Id);
                    if (match != null)
                    {
                        Context.Entry(match).CurrentValues.SetValues(wh);
                    }
                }
                else
                {
                    wh.Space = existingSpace;
                    existingSpace.WorkingHours.Add(wh);
                }
            }

            await Context.SaveChangesAsync();

            return Result.Success();
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.UpdateSpace,
                "DB error updating space {id}. Error: {error}", space.Id, e.Message);

            return Result.Fail($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.UpdateSpace,
                "Error updating space {id}. Error: {error}", space.Id, e.Message);

            return Result.Fail($"Failure updating space: {e.Message}");
        }
    }

    public async Task<Result> DeleteSpaceAsync(int id)
    {
        try
        {
            var space = await Context.Spaces.FindAsync(id);
            if (space is null)
            {
                return Result.Fail($"No space with id: {id}");
            }

            Context.Spaces.Remove(space);
            await Context.SaveChangesAsync();

            return Result.Success();
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.DeleteSpace,
                "DB error deleting space {id}. Error: {error}", id, e.Message);

            return Result.Fail($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, SpaceServiceEventIds.DeleteSpace,
                "Error deleting space {id}. Error: {error}", id, e.Message);

            return Result.Fail($"Failure deleting space: {e.Message}");
        }
    }
}

public class StartEndTime
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}
