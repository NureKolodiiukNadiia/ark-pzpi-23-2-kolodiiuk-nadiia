using Microsoft.EntityFrameworkCore;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Spaces;

public class SpaceService : ISpaceService
{
    private readonly SpotRentDbContext _context;

    public SpaceService(SpotRentDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Space>>> FilterSpacesAsync(Func<Space, bool> cond)
    {
        throw new NotImplementedException();
    }

    // public async Task<Result<IEnumerable<Space>>> GetSpacesAsync(
    //     SpaceType? type,
    //     FilterCriterion[]? filterCriteria,
    //     int? capacity,
    //     int limit,
    //     int offset)
    // {
    //     var query = _context.Spaces.AsQueryable();
    //
    //     if (type.HasValue)
    //     {
    //         query = query.Where(s => s.Type == type.Value);
    //     }
    //
    //     if (capacity.HasValue)
    //     {
    //         query = query.Where(s => s.Capacity >= capacity.Value);
    //     }
    //
    //     if (filterCriteria != null && filterCriteria.Length > 0)
    //     {
    //         foreach (var criterion in filterCriteria)
    //         {
    //             // query = query.Where(space =>
    //             //     space.SpaceAttributes.Any(attr =>
    //             //         attr.AttributeId == criterion.AttributeId &&
    //             //         criterion.AttributeValues.Contains(attr.AttributeValue)
    //             //     )
    //             // );
    //         }
    //     }
    //
    //     var spaces = await query
    //         .Skip(offset)
    //         .Take(limit)
    //         .ToListAsync();
    //
    //     return Result<IEnumerable<Space>>.Success(spaces);
    // }

    public async Task<Result<Space>> GetSpaceByIdAsync(int id)
    {
        var space = await _context.Spaces.FindAsync(id);

        return (space == null)
            ? Result.Fail<Space>("No space with specified id")
            : Result.Success(space);
    }

    public async Task<Result<IEnumerable<Space>>> GetAvailableSpacesAsync(DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Space>> CreateSpaceAsync(Space space)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Space>> UpdateSpaceAsync(Space space)
    {
        var existingSpace = await _context.Spaces.FindAsync(space.Id);
        if (existingSpace == null)
        {
            return Result.Fail<Space>("No space with specified id");
        }

        try
        {
            _context.Entry(existingSpace).CurrentValues.SetValues(space);
            await _context.SaveChangesAsync();

            return Result.Success(existingSpace);
        }
        catch (DbUpdateException e)
        {
            return Result.Fail<Space>($"Update failed: {e.InnerException?.Message ?? e.Message}");
        }
        catch (Exception e)
        {
            return Result.Fail<Space>($"Unexpected error: {e.Message}");
        }
    }

    public async Task<Result> DeleteSpaceAsync(int id)
    {
        var space = await _context.Spaces.FindAsync(id);
        if (space == null)
        {
            return Result.Fail("No space with specified id");
        }

        try
        {
            _context.Remove(space);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Fail($"{e.Message}");
        }

        return Result.Success();
    }

    public async Task<Result<bool>> IsSpaceAvailableAsync(int workspaceId, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Space>> GetSpaceByDeviceIdAsync(string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<SpaceSchedule>> GetSpaceScheduleAsync(int spaceId, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }
}
