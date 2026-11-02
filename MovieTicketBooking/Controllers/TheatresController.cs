using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    // GET: api/theatres
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var theatres = await _theatreService.GetAllAsync();

        return Ok(theatres);
    }

    // GET: api/theatres/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var theatre = await _theatreService.GetByIdAsync(id);

        if (theatre == null)
        {
            return NotFound(new
            {
                message = "Theatre not found."
            });
        }

        return Ok(theatre);
    }

    // POST: api/theatres
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTheatreRequest request)
    {
        var theatre = await _theatreService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = theatre.TheatreId },
            theatre);
    }

    // PUT: api/theatres/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTheatreRequest request)
    {
        var theatre = await _theatreService.UpdateAsync(id, request);

        if (theatre == null)
        {
            return NotFound(new
            {
                message = "Theatre not found."
            });
        }

        return Ok(theatre);
    }
    // DELETE: api/theatres/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _theatreService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Theatre not found."
            });
        }

        return NoContent();
    }
}