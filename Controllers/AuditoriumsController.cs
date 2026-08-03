using CinemaTicketBookingAPI.DTOs.Auditorium;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBookingAPI.Controllers;

[ApiController]
[Route("api/auditoriums")]
public class AuditoriumsController : ControllerBase
{
    private readonly IAuditoriumService _auditoriumService;

    public AuditoriumsController(IAuditoriumService auditoriumService)
    {
        _auditoriumService = auditoriumService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditoriumDto>>> GetAll()
    {
        var auditoriums = await _auditoriumService.GetAllAsync();
        return Ok(auditoriums);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuditoriumDto>> GetById(int id)
    {
        var auditorium = await _auditoriumService.GetByIdAsync(id);
        return Ok(auditorium);
    }

    [HttpPost]
    public async Task<ActionResult<AuditoriumDto>> Create([FromBody] CreateAuditoriumDto dto)
    {
        var created = await _auditoriumService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAuditoriumDto dto)
    {
        await _auditoriumService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _auditoriumService.DeleteAsync(id);
        return NoContent();
    }
}