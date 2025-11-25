using LinqKit;
using Microsoft.EntityFrameworkCore.Query;
using SpotRent.Domain.Entities;

namespace SpotRent.Services.Spaces;

public class SpaceFilterRequest
{
    public ExpressionStarter<Space> Predicate { get; set; }

    public Func<IQueryable<Space>, IIncludableQueryable<Space, object>> Includes { get; set; }

    public Func<IQueryable<Space>, IOrderedQueryable<Space>> OrderBy { get; set; }

    public int SkipCount { get; set; }

    public int? TakeCount { get; set; }

    public Action<int> CaptureTotal { get; set; }
}
