using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingAPI.DTOs.ShowTime;

public class CreateShowTimeDto
{
    [Required]
    public DateTime ShowDateTime { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    public int AuditoriumId { get; set; }
}