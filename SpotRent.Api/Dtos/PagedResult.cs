namespace SpotRent.Api.Dtos;

public record PagedResult<T>(IEnumerable<T> Items, int TotalItems, int Page, int PageSize);
