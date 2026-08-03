namespace CinemaTicketBookingAPI.DTOs.Movie;

public class MovieDtoV2
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public bool AvailableInCinema { get; set; }
}