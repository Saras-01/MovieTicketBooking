using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowsController : ControllerBase
{
    private readonly IShowService _showService;

    public ShowsController(IShowService showService)
    {
        _showService = showService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ShowResponse>>> GetAll()
    {
        var shows = await _showService.GetAllAsync();
        return Ok(shows);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShowResponse>> GetById(int id)
    {
        var show = await _showService.GetByIdAsync(id);

        if (show == null)
        {
            return NotFound(new { message = "Show not found." });
        }

        return Ok(show);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ShowResponse>> Create(
        CreateShowRequest request)
    {
        try
        {
            var show = await _showService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = show.ShowId },
                show);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ShowResponse>> Update(
        int id,
        UpdateShowRequest request)
    {
        try
        {
            var show = await _showService.UpdateAsync(id, request);

            if (show == null)
            {
                return NotFound(new { message = "Show not found." });
            }

            return Ok(show);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _showService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Show not found." });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/seats")]
    public async Task<ActionResult<List<ShowSeatResponse>>> GetShowSeats(
        int id)
    {
        try
        {
            var seats = await _showService.GetShowSeatsAsync(id);
            return Ok(seats);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}