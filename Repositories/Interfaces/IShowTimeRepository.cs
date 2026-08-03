using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Repositories.Interfaces;

public interface IShowTimeRepository
{
    IQueryable<ShowTime> GetQueryable();
    Task<ShowTime?> GetByIdAsync(int id);
    Task<ShowTime?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(ShowTime showTime);
    void Update(ShowTime showTime);
    void Delete(ShowTime showTime);
    Task<bool> HasActiveBookingsAsync(int showTimeId);
    Task<bool> SaveChangesAsync();
}