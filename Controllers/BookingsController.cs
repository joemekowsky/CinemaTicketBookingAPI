using CinemaTicketBookingAPI.DTOs.Booking;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBookingAPI.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // GET api/bookings?customerId=&customerName=&showTimeId=&status=&page=&pageSize=
    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] BookingQueryParams query)
    {
        var result = await _bookingService.GetFilteredAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingDto>> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        return Ok(booking);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto)
    {
        var created = await _bookingService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<ActionResult<BookingDto>> Cancel(int id)
    {
        var cancelled = await _bookingService.CancelAsync(id);
        return Ok(cancelled);
    }
}