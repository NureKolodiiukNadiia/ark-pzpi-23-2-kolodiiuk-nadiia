namespace SpotRent.Api.Dto;

public record PagedResult<T>(IEnumerable<T> Items, int TotalItems, int Page, int PageSize);
