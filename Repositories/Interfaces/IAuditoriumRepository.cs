using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Repositories.Interfaces;

public interface IAuditoriumRepository
{
    IQueryable<Auditorium> GetQueryable();
    Task<Auditorium?> GetByIdAsync(int id);
    Task AddAsync(Auditorium auditorium);
    void Update(Auditorium auditorium);
    void Delete(Auditorium auditorium);
    Task<bool> HasActiveShowTimesAsync(int auditoriumId);
    Task<bool> SaveChangesAsync();
}