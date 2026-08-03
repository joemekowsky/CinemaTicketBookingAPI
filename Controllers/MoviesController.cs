using Asp.Versioning;
using CinemaTicketBookingAPI.DTOs.Movie;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBookingAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/movies")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET api/v1/movies?searchTerm=&genre=&sortBy=&descending=&page=&pageSize=
    // GET api/v2/movies?...
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] MovieQueryParams query)
    {
        var result = await _movieService.GetAllAsync(query);
        var apiVersion = HttpContext.GetRequestedApiVersion();

        if (apiVersion?.MajorVersion == 1)
        {
            var v1Items = result.Items.Select(m => new MovieDtoV1
            {
                Id = m.Id,
                Name = m.Name,
                AvailableInCinema = m.AvailableInCinema
            });

            return Ok(new
            {
                items = v1Items,
                result.Page,
                result.PageSize,
                result.TotalCount,
                result.TotalPages
            });
        }

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        var apiVersion = HttpContext.GetRequestedApiVersion();

        if (apiVersion?.MajorVersion == 1)
        {
            return Ok(new MovieDtoV1
            {
                Id = movie.Id,
                Name = movie.Name,
                AvailableInCinema = movie.AvailableInCinema
            });
        }

        return Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<MovieDtoV2>> Create([FromBody] CreateMovieDto dto)
    {
        var created = await _movieService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id, version = "1.0" }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDto dto)
    {
        await _movieService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _movieService.DeleteAsync(id);
        return NoContent();
    }
}