using CinemaTicketBookingAPI.DTOs;
using CinemaTicketBookingAPI.DTOs.Movie;

namespace CinemaTicketBookingAPI.Services.Interfaces;

public interface IMovieService
{
    Task<PagedResult<MovieDtoV2>> GetAllAsync(MovieQueryParams query);
    Task<MovieDtoV2> GetByIdAsync(int id);
    Task<MovieDtoV2> CreateAsync(CreateMovieDto dto);
    Task UpdateAsync(int id, UpdateMovieDto dto);
    Task DeleteAsync(int id);
}