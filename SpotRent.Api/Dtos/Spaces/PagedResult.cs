namespace SpotRent.Api.Dtos.Spaces;

public record PagedResult<T>(IEnumerable<T> Items, int TotalItems, int Page, int PageSize);
