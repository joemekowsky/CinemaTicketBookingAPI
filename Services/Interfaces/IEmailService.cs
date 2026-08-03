using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Services.Interfaces;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(string toEmail, Booking booking);
    Task SendBookingCancellationAsync(string toEmail, Booking booking);
}