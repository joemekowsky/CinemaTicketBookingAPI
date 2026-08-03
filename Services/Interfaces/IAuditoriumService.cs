using CinemaTicketBookingAPI.DTOs.Auditorium;

namespace CinemaTicketBookingAPI.Services.Interfaces;

public interface IAuditoriumService
{
    Task<IEnumerable<AuditoriumDto>> GetAllAsync();
    Task<AuditoriumDto> GetByIdAsync(int id);
    Task<AuditoriumDto> CreateAsync(CreateAuditoriumDto dto);
    Task UpdateAsync(int id, UpdateAuditoriumDto dto);
    Task DeleteAsync(int id);
}