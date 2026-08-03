namespace CinemaTicketBookingAPI.DTOs.Movie;

public class MovieDtoV1
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool AvailableInCinema { get; set; }
}