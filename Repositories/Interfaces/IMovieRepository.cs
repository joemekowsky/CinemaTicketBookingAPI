using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Repositories.Interfaces;

public interface IMovieRepository
{
    IQueryable<Movie> GetQueryable();
    Task<Movie?> GetByIdAsync(int id);
    Task<Movie?> GetByNameAsync(string name);
    Task AddAsync(Movie movie);
    void Update(Movie movie);
    void Delete(Movie movie);
    Task<bool> HasActiveShowTimesAsync(int movieId);
    Task<bool> SaveChangesAsync();
}