using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingAPI.DTOs.Customer;

public class CreateCustomerDto
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}