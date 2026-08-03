using CinemaTicketBookingAPI.Enums;

namespace CinemaTicketBookingAPI.DTOs.Booking;

public class BookingDto
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }

    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;

    public int ShowTimeId { get; set; }
    public DateTime ShowDateTime { get; set; }
    public string MovieName { get; set; } = string.Empty;
    public int AuditoriumRoomNumber { get; set; }
}