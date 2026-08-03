using CinemaTicketBookingAPI.DTOs;
using CinemaTicketBookingAPI.DTOs.Movie;
using CinemaTicketBookingAPI.Exceptions;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<PagedResult<MovieDtoV2>> GetAllAsync(MovieQueryParams query)
    {
        var movies = _movieRepository.GetQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.Trim().ToLower();
            movies = movies.Where(m => m.Name.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(query.Genre))
        {
            var genre = query.Genre.Trim().ToLower();
            movies = movies.Where(m => m.Genre.ToLower() == genre);
        }

        movies = query.SortBy?.ToLower() switch
        {
            "releasedate" => query.Descending
                ? movies.OrderByDescending(m => m.ReleaseDate)
                : movies.OrderBy(m => m.ReleaseDate),
            "name" => query.Descending
                ? movies.OrderByDescending(m => m.Name)
                : movies.OrderBy(m => m.Name),
            _ => movies.OrderBy(m => m.Id)
        };

        var totalCount = await movies.CountAsync();

        var items = await movies
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(m => ToDtoV2(m))
            .ToListAsync();

        return new PagedResult<MovieDtoV2>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<MovieDtoV2> GetByIdAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id)
            ?? throw new MovieNotFoundException(id);

        return ToDtoV2(movie);
    }

    public async Task<MovieDtoV2> CreateAsync(CreateMovieDto dto)
    {
        var existing = await _movieRepository.GetByNameAsync(dto.Name);
        if (existing is not null)
        {
            throw new MovieAlreadyExistsException(dto.Name);
        }

        var movie = new Movie
        {
            Name = dto.Name,
            Genre = dto.Genre,
            ReleaseDate = dto.ReleaseDate,
            AvailableInCinema = dto.AvailableInCinema
        };

        await _movieRepository.AddAsync(movie);
        await _movieRepository.SaveChangesAsync();

        return ToDtoV2(movie);
    }

    public async Task UpdateAsync(int id, UpdateMovieDto dto)
    {
        var movie = await _movieRepository.GetByIdAsync(id)
            ?? throw new MovieNotFoundException(id);

        var duplicate = await _movieRepository.GetByNameAsync(dto.Name);
        if (duplicate is not null && duplicate.Id != id)
        {
            throw new MovieAlreadyExistsException(dto.Name);
        }

        movie.Name = dto.Name;
        movie.Genre = dto.Genre;
        movie.ReleaseDate = dto.ReleaseDate;
        movie.AvailableInCinema = dto.AvailableInCinema;

        _movieRepository.Update(movie);
        await _movieRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id)
            ?? throw new MovieNotFoundException(id);

        var hasActiveShowTimes = await _movieRepository.HasActiveShowTimesAsync(id);
        if (hasActiveShowTimes)
        {
            // Not one of the 6 named exceptions — this is a referential
            // conflict, mapped to 409 Conflict by the middleware.
            throw new InvalidOperationException(
                $"Movie '{movie.Name}' cannot be deleted because it has active showtimes.");
        }

        _movieRepository.Delete(movie);
        await _movieRepository.SaveChangesAsync();
    }

    private static MovieDtoV2 ToDtoV2(Movie movie) => new()
    {
        Id = movie.Id,
        Name = movie.Name,
        Genre = movie.Genre,
        ReleaseDate = movie.ReleaseDate,
        AvailableInCinema = movie.AvailableInCinema
    };
}