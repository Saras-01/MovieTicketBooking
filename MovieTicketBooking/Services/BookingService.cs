using Microsoft.AspNetCore.Http;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using System.Security.Claims;
using MovieTicketBooking.Exceptions;

namespace MovieTicketBooking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookingService(
        IBookingRepository bookingRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _bookingRepository = bookingRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    // Get all bookings
    public async Task<List<BookingResponse>> GetAllAsync(
        int? showId = null,
        string? status = null)
    {
        var bookings = await _bookingRepository.GetAllAsync(
            showId, status);

        return bookings.Select(MapToResponse).ToList();
    }

    // Get booking by ID
    // Get booking by ID with ownership verification
    public async Task<BookingResponse?> GetByIdAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null ||
            httpContext.User.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            return null;
        }

        // Admin can view any booking
        bool isAdmin = httpContext.User.IsInRole("Admin");

        if (!isAdmin)
        {
            var userIdClaim = httpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException(
                    "Invalid user identity.");
            }

            // Verify booking ownership
            if (booking.UserId != userId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to view this booking.");
            }
        }

        return MapToResponse(booking);
    }

    // Get bookings for a particular show
    public async Task<List<BookingResponse>> GetByShowIdAsync(
        int showId)
    {
        var bookings = await _bookingRepository.GetByShowIdAsync(showId);

        return bookings.Select(MapToResponse).ToList();
    }

    // Create booking
    public async Task<BookingResponse> CreateAsync(
        CreateBookingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            throw new ArgumentException(
                "Customer name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CustomerEmail))
        {
            throw new ArgumentException(
                "Customer email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CustomerPhone))
        {
            throw new ArgumentException(
                "Customer phone is required.");
        }

        if (request.ShowId <= 0)
        {
            throw new ArgumentException(
                "A valid ShowId is required.");
        }

        if (request.SeatIds == null || request.SeatIds.Count == 0)
        {
            throw new ArgumentException(
                "Please select at least one seat.");
        }

        // Get logged-in User ID from JWT token
        var userIdClaim = _httpContextAccessor.HttpContext?
            .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) ||
            !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        var booking = new Booking
        {
            UserId = userId,
            ShowId = request.ShowId,
            CustomerName = request.CustomerName.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            CustomerPhone = request.CustomerPhone.Trim()
        };

        var createdBooking = await _bookingRepository.CreateAsync(
            booking,
            request.SeatIds);

        return MapToResponse(createdBooking);
    }

    // Cancel booking
    // Cancel booking with ownership verification
    public async Task<BookingResponse?> CancelAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null ||
            httpContext.User.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        // Retrieve the booking
        var existingBooking = await _bookingRepository.GetByIdAsync(id);

        if (existingBooking == null)
        {
            return null;
        }

        // Check whether the logged-in user is Admin
        bool isAdmin = httpContext.User.IsInRole("Admin");

        if (!isAdmin)
        {
            var userIdClaim = httpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException(
                    "Invalid user identity.");
            }

            // Verify booking ownership
            if (existingBooking.UserId != userId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to cancel this booking.");
            }
        }

        // Perform existing cancellation operation
        var booking = await _bookingRepository.CancelAsync(id);

        if (booking == null)
        {
            return null;
        }

        return MapToResponse(booking);
    }

    // Convert Booking entity into BookingResponse DTO
    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            CustomerName = booking.CustomerName,
            CustomerEmail = booking.CustomerEmail,
            CustomerPhone = booking.CustomerPhone,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,

            Seats = booking.BookingSeats
                .Where(bs => bs.Seat != null)
                .Select(bs =>
                    $"{bs.Seat!.RowName}{bs.Seat.SeatNumber}")
                .ToList()
        };
    }
}
