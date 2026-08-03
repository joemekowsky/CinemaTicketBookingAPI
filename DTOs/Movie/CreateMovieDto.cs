using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingAPI.DTOs.Movie;

public class CreateMovieDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    public DateTime ReleaseDate { get; set; }

    public bool AvailableInCinema { get; set; }
}