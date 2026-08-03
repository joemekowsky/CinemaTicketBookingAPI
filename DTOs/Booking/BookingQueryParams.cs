using CinemaTicketBookingAPI.Enums;

namespace CinemaTicketBookingAPI.DTOs.Booking;

public class BookingQueryParams
{
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? ShowTimeId { get; set; }
    public BookingStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}