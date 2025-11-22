namespace SpotRent.Api.Dtos;

public class PaginationDto
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public long Total { get; set; }
}
