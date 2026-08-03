using CinemaTicketBookingAPI.DTOs;
using CinemaTicketBookingAPI.DTOs.Booking;
using CinemaTicketBookingAPI.Enums;
using CinemaTicketBookingAPI.Exceptions;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IShowTimeRepository _showTimeRepository;
    private readonly ICustomerService _customerService;
    private readonly IEmailService _emailService;

    public BookingService(
        IBookingRepository bookingRepository,
        IShowTimeRepository showTimeRepository,
        ICustomerService customerService,
        IEmailService emailService)
    {
        _bookingRepository = bookingRepository;
        _showTimeRepository = showTimeRepository;
        _customerService = customerService;
        _emailService = emailService;
    }

    public async Task<PagedResult<BookingDto>> GetFilteredAsync(BookingQueryParams query)
    {
        var bookings = _bookingRepository.GetQueryable();

        if (query.CustomerId.HasValue)
        {
            bookings = bookings.Where(b => b.CustomerId == query.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.CustomerName))
        {
            var name = query.CustomerName.Trim().ToLower();
            bookings = bookings.Where(b => b.Customer.Name.ToLower().Contains(name));
        }

        if (query.ShowTimeId.HasValue)
        {
            bookings = bookings.Where(b => b.ShowTimeId == query.ShowTimeId.Value);
        }

        if (query.Status.HasValue)
        {
            bookings = bookings.Where(b => b.Status == query.Status.Value);
        }

        var totalCount = await bookings.CountAsync();

        var items = await bookings
            .OrderByDescending(b => b.BookingDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(b => ToDto(b))
            .ToListAsync();

        return new PagedResult<BookingDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<BookingDto> GetByIdAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdWithDetailsAsync(id)
            ?? throw new BookingNotFoundException(id);
        return ToDto(booking);
    }

    public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
    {
        // Business rule: a booking cannot be created for a showtime that
        // does not exist.
        var showTime = await _showTimeRepository.GetByIdWithDetailsAsync(dto.ShowTimeId)
            ?? throw new ShowTimeNotFoundException(dto.ShowTimeId);

        // Business rule: a booking must belong to a valid guest customer.
        // Guests have no account, so we find-or-create by email.
        var customer = await _customerService.GetOrCreateGuestAsync(dto.CustomerName, dto.CustomerEmail);

        var booking = new Booking
        {
            BookingDate = DateTime.UtcNow,
            Status = BookingStatus.Pending, // status is always server-controlled
            CustomerId = customer.Id,
            ShowTimeId = showTime.Id
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        await _emailService.SendBookingConfirmationAsync(customer.Email, booking);

        var created = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id)
            ?? throw new BookingNotFoundException(booking.Id);

        return ToDto(created);
    }

    public async Task<BookingDto> ConfirmAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdWithDetailsAsync(id)
            ?? throw new BookingNotFoundException(id);

        if (booking.Status == BookingStatus.Cancelled)
        {
            throw new InvalidBookingException("A cancelled booking cannot be confirmed.");
        }

        booking.Status = BookingStatus.Confirmed;
        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();

        return ToDto(booking);
    }

    public async Task<BookingDto> CancelAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdWithDetailsAsync(id)
            ?? throw new BookingNotFoundException(id);

        if (booking.Status == BookingStatus.Cancelled)
        {
            throw new InvalidBookingException("This booking is already cancelled.");
        }

        booking.Status = BookingStatus.Cancelled;
        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();

        await _emailService.SendBookingCancellationAsync(booking.Customer.Email, booking);

        return ToDto(booking);
    }

    private static BookingDto ToDto(Booking booking) => new()
    {
        Id = booking.Id,
        BookingDate = booking.BookingDate,
        Status = booking.Status,
        CustomerId = booking.CustomerId,
        CustomerName = booking.Customer?.Name ?? string.Empty,
        CustomerEmail = booking.Customer?.Email ?? string.Empty,
        ShowTimeId = booking.ShowTimeId,
        ShowDateTime = booking.ShowTime?.ShowTimeDate ?? default,
        MovieName = booking.ShowTime?.Movie?.Name ?? string.Empty,
        AuditoriumRoomNumber = booking.ShowTime?.Auditorium?.RoomNumber ?? 0
    };
}