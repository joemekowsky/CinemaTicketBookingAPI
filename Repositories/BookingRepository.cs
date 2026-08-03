using CinemaTicketBookingAPI.Data;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly CinemaDbContext _context;

    public BookingRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public IQueryable<Booking> GetQueryable() =>
        _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.ShowTime).ThenInclude(s => s.Movie)
            .Include(b => b.ShowTime).ThenInclude(s => s.Auditorium)
            .AsQueryable();

    public async Task<Booking?> GetByIdAsync(int id) =>
        await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Booking?> GetByIdWithDetailsAsync(int id) =>
        await GetQueryable().FirstOrDefaultAsync(b => b.Id == id);

    public void Update(Booking booking) => _context.Bookings.Update(booking);

    public async Task AddAsync(Booking booking) => await _context.Bookings.AddAsync(booking);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}