using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingAPI.DTOs.Booking;


public class CreateBookingDto
{
    [Required]
    public int ShowTimeId { get; set; }

    [Required, MaxLength(150)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;
}