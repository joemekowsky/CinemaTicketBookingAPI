using CinemaTicketBookingAPI.DTOs.ShowTime;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBookingAPI.Controllers;

[ApiController]
[Route("api/showtimes")]
public class ShowTimesController : ControllerBase
{
    private readonly IShowTimeService _showTimeService;

    public ShowTimesController(IShowTimeService showTimeService)
    {
        _showTimeService = showTimeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowTimeDto>>> GetAll()
    {
        var showTimes = await _showTimeService.GetAllAsync();
        return Ok(showTimes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShowTimeDto>> GetById(int id)
    {
        var showTime = await _showTimeService.GetByIdAsync(id);
        return Ok(showTime);
    }

    [HttpPost]
    public async Task<ActionResult<ShowTimeDto>> Create([FromBody] CreateShowTimeDto dto)
    {
        var created = await _showTimeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _showTimeService.DeleteAsync(id);
        return NoContent();
    }
}