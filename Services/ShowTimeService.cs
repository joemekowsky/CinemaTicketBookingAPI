using CinemaTicketBookingAPI.DTOs.ShowTime;
using CinemaTicketBookingAPI.Exceptions;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Services;

public class ShowTimeService : IShowTimeService
{
    private readonly IShowTimeRepository _showTimeRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IAuditoriumRepository _auditoriumRepository;

    public ShowTimeService(
        IShowTimeRepository showTimeRepository,
        IMovieRepository movieRepository,
        IAuditoriumRepository auditoriumRepository)
    {
        _showTimeRepository = showTimeRepository;
        _movieRepository = movieRepository;
        _auditoriumRepository = auditoriumRepository;
    }

    public async Task<IEnumerable<ShowTimeDto>> GetAllAsync() =>
        await _showTimeRepository.GetQueryable()
            .Include(s => s.Movie)
            .Include(s => s.Auditorium)
            .Select(s => ToDto(s))
            .ToListAsync();

    public async Task<ShowTimeDto> GetByIdAsync(int id)
    {
        var showTime = await _showTimeRepository.GetByIdWithDetailsAsync(id)
            ?? throw new ShowTimeNotFoundException(id);
        return ToDto(showTime);
    }

    public async Task<ShowTimeDto> CreateAsync(CreateShowTimeDto dto)
    {
        // Business rule: a movie referenced by a showtime must exist.
        var movie = await _movieRepository.GetByIdAsync(dto.MovieId)
            ?? throw new MovieNotFoundException(dto.MovieId);

        // Business rule: an auditorium referenced by a showtime must exist.
        var auditorium = await _auditoriumRepository.GetByIdAsync(dto.AuditoriumId)
            ?? throw new KeyNotFoundException($"Auditorium with id {dto.AuditoriumId} was not found.");

        // Business rule: a movie can only be scheduled according to its
        // cinema availability.
        if (!movie.AvailableInCinema)
        {
            throw new InvalidBookingException(
                $"Movie '{movie.Name}' is not currently available in cinema and cannot be scheduled.");
        }

        var showTime = new ShowTime
        {
            ShowTimeDate = dto.ShowDateTime,
            MovieId = dto.MovieId,
            AuditoriumId = dto.AuditoriumId
        };

        await _showTimeRepository.AddAsync(showTime);
        await _showTimeRepository.SaveChangesAsync();

        return new ShowTimeDto
        {
            Id = showTime.Id,
            ShowDateTime = showTime.ShowTimeDate,
            MovieId = movie.Id,
            MovieName = movie.Name,
            AuditoriumId = auditorium.Id,
            AuditoriumRoomNumber = auditorium.RoomNumber
        };
    }

    public async Task DeleteAsync(int id)
    {
        var showTime = await _showTimeRepository.GetByIdAsync(id)
            ?? throw new ShowTimeNotFoundException(id);

        var hasActiveBookings = await _showTimeRepository.HasActiveBookingsAsync(id);
        if (hasActiveBookings)
        {
            throw new InvalidOperationException(
                "This showtime cannot be deleted because it has active bookings.");
        }

        _showTimeRepository.Delete(showTime);
        await _showTimeRepository.SaveChangesAsync();
    }

    private static ShowTimeDto ToDto(ShowTime showTime) => new()
    {
        Id = showTime.Id,
        ShowDateTime = showTime.ShowTimeDate,
        MovieId = showTime.MovieId,
        MovieName = showTime.Movie?.Name ?? string.Empty,
        AuditoriumId = showTime.AuditoriumId,
        AuditoriumRoomNumber = showTime.Auditorium?.RoomNumber ?? 0
    };
}