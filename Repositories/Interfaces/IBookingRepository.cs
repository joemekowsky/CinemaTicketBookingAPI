using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Repositories.Interfaces;

public interface IBookingRepository
{
    IQueryable<Booking> GetQueryable();
    Task<Booking?> GetByIdAsync(int id);
    Task<Booking?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Booking booking);
    void Update(Booking booking);
    Task<bool> SaveChangesAsync();
}