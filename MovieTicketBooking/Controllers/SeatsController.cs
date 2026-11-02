using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    // GET: api/seats
    [HttpGet]
    public async Task<ActionResult<List<SeatResponse>>> GetAll()
    {
        var seats = await _seatService.GetAllAsync();

        return Ok(seats);
    }

    // GET: api/seats/1
    [HttpGet("{id}")]
    public async Task<ActionResult<SeatResponse>> GetById(int id)
    {
        var seat = await _seatService.GetByIdAsync(id);

        if (seat == null)
        {
            return NotFound(new { message = "Seat not found." });
        }

        return Ok(seat);
    }

    // POST: api/seats
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<SeatResponse>> Create(
        CreateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = seat.SeatId },
                seat);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/seats/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<SeatResponse>> Update(
        int id,
        UpdateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.UpdateAsync(id, request);

            if (seat == null)
            {
                return NotFound(new { message = "Seat not found." });
            }

            return Ok(seat);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/seats/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _seatService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new { message = "Seat not found." });
        }

        return NoContent();
    }
}