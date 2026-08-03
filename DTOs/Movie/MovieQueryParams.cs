namespace CinemaTicketBookingAPI.DTOs.Movie;

public class MovieQueryParams
{
    public string? SearchTerm { get; set; }
    public string? Genre { get; set; }
    public string? SortBy { get; set; }
    public bool Descending { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}