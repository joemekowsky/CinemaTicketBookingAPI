using CinemaTicketBookingAPI.Data;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly CinemaDbContext _context;

    public MovieRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public IQueryable<Movie> GetQueryable() => _context.Movies.AsQueryable();

    public async Task<Movie?> GetByIdAsync(int id) =>
        await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

    public async Task<Movie?> GetByNameAsync(string name) =>
        await _context.Movies.FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower());

    public async Task AddAsync(Movie movie) => await _context.Movies.AddAsync(movie);

    public void Update(Movie movie) => _context.Movies.Update(movie);

    public void Delete(Movie movie) => _context.Movies.Remove(movie);

    public async Task<bool> HasActiveShowTimesAsync(int movieId) =>
        await _context.ShowTimes.AnyAsync(s => s.MovieId == movieId);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}