using CinemaTicketBookingAPI.DTOs.Auditorium;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Services;

public class AuditoriumService : IAuditoriumService
{
    private readonly IAuditoriumRepository _auditoriumRepository;

    public AuditoriumService(IAuditoriumRepository auditoriumRepository)
    {
        _auditoriumRepository = auditoriumRepository;
    }

    public async Task<IEnumerable<AuditoriumDto>> GetAllAsync() =>
        await _auditoriumRepository.GetQueryable()
            .Select(a => ToDto(a))
            .ToListAsync();

    public async Task<AuditoriumDto> GetByIdAsync(int id)
    {
        // Note: the spec's 6 named exceptions don't include an
        // "AuditoriumNotFoundException" — KeyNotFoundException is used
        // instead, mapped to 404 by the middleware, same as the named ones.
        var auditorium = await _auditoriumRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Auditorium with id {id} was not found.");
        return ToDto(auditorium);
    }

    public async Task<AuditoriumDto> CreateAsync(CreateAuditoriumDto dto)
    {
        var auditorium = new Auditorium
        {
            RoomNumber = dto.RoomNumber,
            Capacity = dto.Capacity,
            Available = dto.Available
        };

        await _auditoriumRepository.AddAsync(auditorium);
        await _auditoriumRepository.SaveChangesAsync();

        return ToDto(auditorium);
    }

    public async Task UpdateAsync(int id, UpdateAuditoriumDto dto)
    {
        var auditorium = await _auditoriumRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Auditorium with id {id} was not found.");

        auditorium.RoomNumber = dto.RoomNumber;
        auditorium.Capacity = dto.Capacity;
        auditorium.Available = dto.Available;

        _auditoriumRepository.Update(auditorium);
        await _auditoriumRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var auditorium = await _auditoriumRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Auditorium with id {id} was not found.");

        var hasActiveShowTimes = await _auditoriumRepository.HasActiveShowTimesAsync(id);
        if (hasActiveShowTimes)
        {
            throw new InvalidOperationException(
                $"Auditorium '{auditorium.RoomNumber}' cannot be deleted because it has active showtimes.");
        }

        _auditoriumRepository.Delete(auditorium);
        await _auditoriumRepository.SaveChangesAsync();
    }

    private static AuditoriumDto ToDto(Auditorium auditorium) => new()
    {
        Id = auditorium.Id,
        RoomNumber = auditorium.RoomNumber,
        Capacity = auditorium.Capacity,
        Available = auditorium.Available
    };
}