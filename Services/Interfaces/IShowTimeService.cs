using CinemaTicketBookingAPI.DTOs.ShowTime;

namespace CinemaTicketBookingAPI.Services.Interfaces;

public interface IShowTimeService
{
    Task<IEnumerable<ShowTimeDto>> GetAllAsync();
    Task<ShowTimeDto> GetByIdAsync(int id);
    Task<ShowTimeDto> CreateAsync(CreateShowTimeDto dto);
    Task DeleteAsync(int id);
}