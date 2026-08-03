using CinemaTicketBookingAPI.Data;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Repositories;

public class ShowTimeRepository : IShowTimeRepository
{
    private readonly CinemaDbContext _context;

    public ShowTimeRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public IQueryable<ShowTime> GetQueryable() => _context.ShowTimes.AsQueryable();

    public async Task<ShowTime?> GetByIdAsync(int id) =>
        await _context.ShowTimes.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<ShowTime?> GetByIdWithDetailsAsync(int id) =>
        await _context.ShowTimes
            .Include(s => s.Movie)
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task AddAsync(ShowTime showTime) => await _context.ShowTimes.AddAsync(showTime);

    public void Update(ShowTime showTime) => _context.ShowTimes.Update(showTime);

    public void Delete(ShowTime showTime) => _context.ShowTimes.Remove(showTime);

    public async Task<bool> HasActiveBookingsAsync(int showTimeId) =>
        await _context.Bookings.AnyAsync(b => b.ShowTimeId == showTimeId);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}