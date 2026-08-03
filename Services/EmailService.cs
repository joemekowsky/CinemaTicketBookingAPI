using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Services.Interfaces;

namespace CinemaTicketBookingAPI.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendBookingConfirmationAsync(string toEmail, Booking booking)
    {
        _logger.LogInformation(
            "Booking confirmation email sent to {Email} for booking #{BookingId}.",
            toEmail, booking.Id);
        return Task.CompletedTask;
    }

    public Task SendBookingCancellationAsync(string toEmail, Booking booking)
    {
        _logger.LogInformation(
            "Booking cancellation email sent to {Email} for booking #{BookingId}.",
            toEmail, booking.Id);
        return Task.CompletedTask;
    }
}