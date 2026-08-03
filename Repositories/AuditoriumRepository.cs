using CinemaTicketBookingAPI.Data;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Repositories;

public class AuditoriumRepository : IAuditoriumRepository
{
    private readonly CinemaDbContext _context;

    public AuditoriumRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public IQueryable<Auditorium> GetQueryable() => _context.Auditoriums.AsQueryable();

    public async Task<Auditorium?> GetByIdAsync(int id) =>
        await _context.Auditoriums.FirstOrDefaultAsync(a => a.Id == id);

    public async Task AddAsync(Auditorium auditorium) => await _context.Auditoriums.AddAsync(auditorium);

    public void Update(Auditorium auditorium) => _context.Auditoriums.Update(auditorium);

    public void Delete(Auditorium auditorium) => _context.Auditoriums.Remove(auditorium);

    public async Task<bool> HasActiveShowTimesAsync(int auditoriumId) =>
        await _context.ShowTimes.AnyAsync(s => s.AuditoriumId == auditoriumId);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}