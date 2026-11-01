using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // GET: api/bookings
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? showId,
        [FromQuery] string? status)
    {
        var bookings = await _bookingService.GetAllAsync(showId, status);

        return Ok(bookings);
    }

    // GET: api/bookings/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }

    // GET: api/bookings/show/1
    [HttpGet("show/{showId}")]
    public async Task<IActionResult> GetByShowId(int showId)
    {
        var bookings = await _bookingService.GetByShowIdAsync(showId);

        return Ok(bookings);
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request)
    {
        var booking = await _bookingService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = booking.BookingId },
            booking);
    }

    // PUT: api/bookings/1/cancel
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _bookingService.CancelAsync(id);

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }
}