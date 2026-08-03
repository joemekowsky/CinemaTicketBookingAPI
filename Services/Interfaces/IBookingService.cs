using CinemaTicketBookingAPI.DTOs;
using CinemaTicketBookingAPI.DTOs.Booking;

namespace CinemaTicketBookingAPI.Services.Interfaces;

public interface IBookingService
{
    Task<PagedResult<BookingDto>> GetFilteredAsync(BookingQueryParams query);
    Task<BookingDto> GetByIdAsync(int id);
    Task<BookingDto> CreateAsync(CreateBookingDto dto);
    Task<BookingDto> ConfirmAsync(int id);
    Task<BookingDto> CancelAsync(int id);
}